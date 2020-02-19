using Amazon;
using Amazon.CognitoIdentity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LostAndFound.Util
{
    class AmazonUtils
    {
        private static CognitoAWSCredentials _credentials;

        public static CognitoAWSCredentials Credentials
        {
            get
            {
                if(_credentials == null)
                {
                    // Initialize the Amazon Cognito credentials provider
                    _credentials = new CognitoAWSCredentials("us-east-2:79d460fe-0b66-4cc8-82b4-6fa77748f00c", // Identity pool ID 
                        RegionEndpoint.USEast2 // Region
                        );

                }

                return _credentials;
            }

        }
            
    }
}
