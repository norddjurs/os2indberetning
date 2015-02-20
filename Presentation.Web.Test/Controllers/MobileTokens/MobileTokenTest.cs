﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Core.ApplicationServices;
using Core.ApplicationServices.Interfaces;
using Core.DomainModel;
using Core.DomainServices;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OS2Indberetning;
using OS2Indberetning.App_Start;
using Owin;
using Presentation.Web.Test.Controllers.Models;

namespace Presentation.Web.Test.Controllers.MobileTokens
{
    [TestClass]
    public class MobileTokenTest
    {
        protected TestServer Server;

        [TestInitialize]
        public void Setup()
        {
            Server = TestServer.Create(app =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                config.DependencyResolver = new NinjectDependencyResolver(NinjectTestInjector.CreateKernel(GetInjections()));
                app.UseWebApi(config);
            });
            //Bit of a hack to make sure that the repository is seeded
            //before each test, but at the same time that it does not 
            //seed each time it is loaded which forgets state if it is
            //queried multiple times during a single test
            ReSeed();
        }

        [TestCleanup]
        public void TearDown()
        {
            if (Server != null)
                Server.Dispose();
        }

        [TestMethod]
        public async Task GetShouldReturnNoElements()
        {
            HttpResponseMessage response = await Server.CreateRequest(GetUriPath()).GetAsync();
            BaseControllerTest<MobileToken>.AssertEmptyResponse(response); //MobileToken controller does not allow this call so it returns an empty list
        }

        [TestMethod]
        public async Task GetWithKeyShouldReturnAllTokensForPersonWithId()
        {
            HttpResponseMessage response = await Server.CreateRequest(GetUriPath() + "(1)").GetAsync();
            var result = await response.Content.ReadAsAsync<ODataResponse<MobileToken>>();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Response to get request should be OK");
            Assert.AreEqual(2, result.value.Count, "Expects the return of a get request with key is to have two mobile tokens");
            AsssertEqualEntities(GetReferenceEntity1(), result.value[0]);
            AsssertEqualEntities(GetReferenceEntity2(), result.value[1]);
        }

        [TestMethod]
        public async Task GetWithInvalidKeyShouldReturnNoEntity()
        {
            HttpResponseMessage response = await Server.CreateRequest(GetUriPath() + "(5)").GetAsync();
            BaseControllerTest<MobileToken>.AssertEmptyResponse(response);
        }

        [TestMethod]
        public async Task PutShouldReturnMethodNotAllowed()
        {
            var httpContent = new StreamContent(Stream.Null);
            var response = await Server.HttpClient.PutAsync(GetUriPath() + "(3)", httpContent);
            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.StatusCode, "Put method should not be allowed");
        }

        [TestMethod]
        public async Task PatchShouldNotBeAllowed()
        {
            var request = Server.CreateRequest(GetUriPath() + "(3)")
                    .And(r => r.Content = new StringContent(GetPatchBodyContent()))
                    .And(r => r.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json"));
            var patchResponse = await request.SendAsync("PATCH");
            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, patchResponse.StatusCode, "Patch is not allowed for mobile token");
        }


        [TestMethod]
        public async Task DeleteShouldRemoveAMobileToken()
        {
            //Make sure that an entity with person Id 2
            HttpResponseMessage response = await Server.CreateRequest(GetUriPath() + "(2)").GetAsync();
            var result = await response.Content.ReadAsAsync<ODataResponse<MobileToken>>();
            Assert.AreEqual(1, result.value.Count, "There should be excatly one entity with id 3 before the patch");

            var request = Server.CreateRequest(GetUriPath() + "(3)");
            var patchResponse = await request.SendAsync("DELETE");
            Assert.AreEqual(HttpStatusCode.OK, patchResponse.StatusCode, "Delete request should return status code OK");

            //After the delete the repo should not contain an entity with id 3
            response = await Server.CreateRequest(GetUriPath() + "(2)").GetAsync();
            BaseControllerTest<MobileToken>.AssertEmptyResponse(response);
        }

        protected void AsssertEqualEntities(MobileToken m1, MobileToken m2)
        {
            Assert.AreEqual(m1.Id, m2.Id, "ID of two mobile tokens does not match");
            Assert.AreEqual(m1.Token, m2.Token, "Token of two mobile tokens does not match");
            Assert.AreEqual(m1.Status, m2.Status, "Status of two mobile tokens does not match");
        }


        protected List<KeyValuePair<Type, Type>> GetInjections()
        {
            return new List<KeyValuePair<Type, Type>>
            {
                new KeyValuePair<Type, Type>(typeof (IGenericRepository<MobileToken>), typeof (MobileTokenRepositoryMock)),
                new KeyValuePair<Type, Type>(typeof (IMobileTokenService), typeof(MobileTokenService))
            };
        }

        protected MobileToken GetReferenceEntity1()
        {
            return new MobileToken
            {
                Id = 1,
                Token = "token1",
                Status = MobileTokenStatus.Activated
            };
        }

        protected MobileToken GetReferenceEntity2()
        {
            return new MobileToken
            {
                Id = 2,
                Token = "token2",
                Status = MobileTokenStatus.Created
            };
        }

        protected MobileToken GetReferenceEntity3()
        {
            return new MobileToken
            {
                Id = 3,
                Token = "token3",
                Status = MobileTokenStatus.Deleted
            };
        }

        protected string GetPatchBodyContent()
        {
            return @"{
                        'Token' : 'patched token',
                        'Status' : 'Activated'
                    }";
        }

        protected string GetUriPath()
        {
            return "/odata/MobileToken";
        }

        protected void ReSeed()
        {
            new MobileTokenRepositoryMock().ReSeed();
        }
    }
}