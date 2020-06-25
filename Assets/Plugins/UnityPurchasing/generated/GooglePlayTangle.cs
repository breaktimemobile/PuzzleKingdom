#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("Orm3uIg6ubK6Orm5uBtd39EGreO4ejRcJlYTHcacIJ/QCAelrteCCc7n2sY3hInjcDhAWjfaOor+Fz+k70NvvHKtESyZcOGyTCsYK2gu+4M3WK5fl4jo+kdUvDDbZFZWjhZfu0b2nwg1lVREFBdhQPepCssPYH/1C4TuFVsGTSNsX1/67dm+Q5M8OnFmQAmy2FJ5q8EWn2JdbP3lyvx3mIeM8fHm3oM+1OcEL3UhD7qp2BecerEofTrdkX6mY2e7EVmZIUJsaoRTMR8C++EG3R/ZlhDGKLuSkPc/4UspBTzk3WRBPCbxsvJLtqpfjmFaiDq5moi1vrGSPvA+T7W5ubm9uLsyerXaSYNADVJ/T/w63JV992BZL+LOF8aSEeQ917q7ubi5");
        private static int[] order = new int[] { 1,13,12,3,9,8,10,7,10,11,10,11,13,13,14 };
        private static int key = 184;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
