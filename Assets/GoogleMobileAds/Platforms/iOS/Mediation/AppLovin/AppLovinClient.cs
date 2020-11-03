﻿// Copyright (C) 2017 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if UNITY_IOS

using UnityEngine;

using GoogleMobileAds.Common.Mediation.AppLovin;

namespace GoogleMobileAds.iOS.Mediation.AppLovin
{
    public class AppLovinClient: IAppLovinClient
    {
        private static AppLovinClient instance = new AppLovinClient();

        private AppLovinClient() {}

        public static AppLovinClient Instance
        {
            get
            {
                return instance;
            }
        }

        public void Initialize()
        {
            Externs.GADUMInitializeAppLovin();
        }

        public void SetHasUserConsent(bool hasUserConsent)
        {
            string parameterString = (hasUserConsent == true ? "YES" : "NO");
            MonoBehaviour.print("Calling '[ALPrivacySettings setHasUserConsent:]' with argument: " + parameterString);
            Externs.GADUMAppLovinSetHasUserConsent (hasUserConsent);
        }

        public void SetIsAgeRestrictedUser(bool isAgeRestrictedUser)
        {
            string parameterString = (isAgeRestrictedUser == true ? "YES" : "NO");
            MonoBehaviour.print("Calling '[ALPrivacySettings setIsAgeRestrictedUser:]' with argument: " + parameterString);
            Externs.GADUMAppLovinSetIsAgeRestrictedUser (isAgeRestrictedUser);
        }
    }
}

#endif
