﻿declare module server {
	interface Person {
		id: number;
		cprNumber: string;
		personId: number;
		firstName: string;
		lastName: string;
		mail: string;
		recieveMail: boolean;
		distanceFromHomeToBorder: number;
		initials: string;
		fullName: string;
		isAdmin: boolean;
		isSubstitute: boolean;
		personalAddresses: any[];
		personalRoutes: any[];
		licensePlates: any[];
		mobileTokens: any[];
		reports: any[];
		employments: any[];
		substitutes: any[];
		substituteFor: any[];
		substituteLeaders: any[];
	}
}
