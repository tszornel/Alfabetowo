// WARNING: Do not modify! Generated file.

using System;

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("d8Nn+lEHzEMeG8VvNEDY+2KFQLk6ube4iDq5sro6ubm4KeMg4NVoVrsGqUQG8AdQfS+y4PdErci+phsUEjuqSPm7UWpjfGKcpDWbqWOk0wGw5I5nfB0VM01JtygomGa2Mzr1A9qCrq0+1x01/5oQ92qDOwnsVIF6gouSV2SNugCFNZYjzydI1ioSNhZ5JnwdpPxqeo04gZxWSAAiuTbCncCutbks4612wKHYe65mJS8EobiFiDq5moi1vrGSPvA+T7W5ubm9uLvUh95nFBPR6kGndb4Uo/FgNFOjHfA7A7tVHSTXsR5bWWE+sda1/QGfiH8zm5/GgP+QEACzF0vVzUjNqRHTe5o3YUnVUZsHEOudpR/CrlknyYLY6saOnMCeN7q7ubi5");
        private static int[] order = new int[] { 3,1,11,10,4,9,9,7,9,10,12,11,12,13,14 };
        private static int key = 184;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }

    internal class Obfuscator
    {
        internal static byte[] DeObfuscate(byte[] data, int[] order, int key)
        {
            throw new NotImplementedException();
        }
    }
}
