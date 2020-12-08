using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace NanoDecrypt
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr FindResourceEx(IntPtr intptr_0, int int_0, int int_1, short short_0);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool EnumResourceNames(IntPtr hModule, string lpType, IntPtr lpEnumFunc, IntPtr lParam);

        // Token: 0x04000064 RID: 100
        private static ICryptoTransform Encryptor;

        // Token: 0x04000065 RID: 101
        private static ICryptoTransform Decryptor;

        public struct GStruct2
        {
            // Token: 0x0400006F RID: 111
            public byte byte_0;

            // Token: 0x04000070 RID: 112
            public byte byte_1;

            // Token: 0x04000071 RID: 113
            public Guid guid_0;

            // Token: 0x04000072 RID: 114
            public object[] object_0;
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                 PrintHelp();
            }
            else
            {
                ProcessFile(args[0], args[1]);
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("> NanoDump");
            Console.WriteLine(">> Automatically extract plugins and config from NanoCore");
            Console.WriteLine(">> @hariomenkel / https://github.com/hariomenkel/NanoDump");
            Console.WriteLine("Usage: " + System.AppDomain.CurrentDomain.FriendlyName + " <Input File> <Output Folder>");
            Console.WriteLine("Example: " + AppDomain.CurrentDomain.FriendlyName + @" NanoCore.exe C:\Users\Vladimir\Desktop\NanoCoreDump");
        }

        static void ProcessFile(string input, string output)
        {
            Guid guid;
            Assembly a = null;

            // Extract the Guid from the sample which is used as Key and IV to get the actual decryption key from the
            // resource file. This is not really necessary because the Guid differs from build to build but the underlying
            // key remains constant. Since I could only test with one version it seems safer to do this step if this
            // key ever changes
            try
            {
                a = Assembly.LoadFile(input);
                guid = new Guid(((GuidAttribute)a.GetCustomAttributes(typeof(GuidAttribute), false)[0]).Value);
                Console.WriteLine("Successfully extracted Guid from file: " + guid.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't extract Guid from sample: " + ex.ToString());
                return;
            }

            // We now load the native resource
            byte[] resource = GetResourceFromExecutable(input, "1", "RCData");
            if (resource != null)
            {
                MemoryStream ms = new MemoryStream(resource);
                BinaryReader binaryReader = new BinaryReader(ms);
                byte[] byte_ = binaryReader.ReadBytes(binaryReader.ReadInt32());
                byte[] key = GetKeyFromResourceFile(byte_, guid);
                InitializeEncryptorDecryptor(key);

                byte[] byte_2 = binaryReader.ReadBytes(binaryReader.ReadInt32());
                
                object[] array = smethod_2(byte_2);
                int num = 0;
                object[] plugins = new object[(int)array[num]];
                num++;
                Array.Copy(array, num, plugins, 0, plugins.Length);
                num += plugins.Length;
                object[] configuration = new object[(int)array[num]];
                num++;
                Array.Copy(array, num, configuration, 0, configuration.Length);

                var dict = new Dictionary<string, string>();

                int i = 0;
                while (i < configuration.Length)
                {
                    Console.WriteLine(configuration[i].ToString() + ": " + configuration[i + 1].ToString());
                    dict.Add(configuration[i].ToString(), configuration[i + 1].ToString());
                    i = i + 2;
                }

                Log(output, dict);
                DumpPlugins(plugins, output);
                Console.WriteLine("Finished!");
            }
            else
            {
                Console.WriteLine("Couldn't find encrypted resource file!");
            }            
        }

        static void Log(string path, Dictionary<string, string> config)
        {
            string logfilePath = "";
            if (path.EndsWith(@"\"))
            {
                logfilePath = path + "NanoCore_Config.txt";
            }
            else
            {
                logfilePath = path + @"\" + "NanoCore_Config.txt";
            }

            var output = JsonConvert.SerializeObject(config);

            using (StreamWriter sw = File.CreateText(logfilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(sw, output);
            }           
        }

        static void DumpPlugins(object[] plugins, string path)
        {
            // First two objects are related to ClientPlugin
            File.WriteAllBytes(path + "/ClientPlugin.dll", (byte[])plugins[1]);

            int numPlugins = (plugins.Length - 2) / 4;
            Console.WriteLine("Found " + numPlugins + " Plugins");

            // We start to read at position 2 since the first two entries are the core plugin
            List<Plugin> pluginList = new List<Plugin>();
            int offset = 2;
            for (int i = 0; i < numPlugins; i++)
            {
                Plugin p = new Plugin();
                p.Guid = (Guid)plugins[offset];
                p.LastUpdated = (DateTime)plugins[offset + 1];
                p.Name = (string)plugins[offset + 2];
                p.Payload = (byte[])plugins[offset + 3];
                pluginList.Add(p);
                offset += 4;
            }

            foreach (Plugin p in pluginList)
            {
                Console.WriteLine("Dumping plugin '" + p.Name + "'");
                File.WriteAllBytes(path + "/" + p.Name + ".dll", p.Payload);
            }
        }

        public static object[] smethod_2(byte[] byte_0)
        {
            return smethod_4(byte_0).object_0;
        }

        private static byte[] GetKeyFromResourceFile(byte[] byte_3, Guid guid_0)
        {
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(guid_0.ToByteArray(), guid_0.ToByteArray(), 8);
            return new RijndaelManaged
            {
                IV = rfc2898DeriveBytes.GetBytes(16),
                Key = rfc2898DeriveBytes.GetBytes(16)
            }.CreateDecryptor().TransformFinalBlock(byte_3, 0, byte_3.Length);
        }

        public static void InitializeEncryptorDecryptor(byte[] byte_0)
        {
            DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider();
            descryptoServiceProvider.BlockSize = 64;
            descryptoServiceProvider.Key = byte_0;
            descryptoServiceProvider.IV = byte_0;
            Encryptor = descryptoServiceProvider.CreateEncryptor();
            Decryptor = descryptoServiceProvider.CreateDecryptor();
        }

        public static byte[] GetResourceFromExecutable(string lpFileName, string lpName, string lpType)
        {
            IntPtr hModule = LoadLibrary(lpFileName);
            if (hModule != IntPtr.Zero)
            {
                IntPtr hResource = FindResourceEx(hModule, 10, 1, 0);
                if (hResource != IntPtr.Zero)
                {
                    uint resSize = SizeofResource(hModule, hResource);
                    IntPtr resData = LoadResource(hModule, hResource);
                    if (resData != IntPtr.Zero)
                    {
                        byte[] uiBytes = new byte[resSize];
                        IntPtr ipMemorySource = LockResource(resData);
                        Marshal.Copy(ipMemorySource, uiBytes, 0, (int)resSize);
                        return uiBytes;
                    }
                }
            }
            return null;
        }

        public static GStruct2 smethod_4(byte[] byte_0)
        {
            List<object> list_0 = new List<object>();

            object obj = RuntimeHelpers.GetObjectValue(new object());
            ObjectFlowControl.CheckForSyncLockOnValueType(obj);
            GStruct2 result;
            lock (obj)
            {
                byte_0 = Decryptor.TransformFinalBlock(byte_0, 0, byte_0.Length);
                MemoryStream memoryStream_0 = new MemoryStream(byte_0);
                BinaryReader binaryReader_0 = new BinaryReader(memoryStream_0);
                if (binaryReader_0.ReadBoolean())
                {
                    int num = binaryReader_0.ReadInt32();
                    DeflateStream deflateStream = new DeflateStream(memoryStream_0, CompressionMode.Decompress, false);
                    byte[] array = new byte[num - 1 + 1];
                    deflateStream.Read(array, 0, array.Length);
                    deflateStream.Close();
                    memoryStream_0 = new MemoryStream(array);
                    binaryReader_0 = new BinaryReader(memoryStream_0);
                }
                GStruct2 gstruct = default(GStruct2);
                gstruct.byte_0 = binaryReader_0.ReadByte();
                gstruct.byte_1 = binaryReader_0.ReadByte();
                if (binaryReader_0.ReadBoolean())
                {
                    gstruct.guid_0 = new Guid(binaryReader_0.ReadBytes(16));
                }
                while (memoryStream_0.Position != memoryStream_0.Length)
                {
                    switch (binaryReader_0.ReadByte())
                    {
                        case 0:
                            list_0.Add(binaryReader_0.ReadBoolean());
                            break;
                        case 1:
                            list_0.Add(binaryReader_0.ReadByte());
                            break;
                        case 2:
                            list_0.Add(binaryReader_0.ReadBytes(binaryReader_0.ReadInt32()));
                            break;
                        case 3:
                            list_0.Add(binaryReader_0.ReadChar());
                            break;
                        case 4:
                            list_0.Add(binaryReader_0.ReadString().ToCharArray());
                            break;
                        case 5:
                            list_0.Add(binaryReader_0.ReadDecimal());
                            break;
                        case 6:
                            list_0.Add(binaryReader_0.ReadDouble());
                            break;
                        case 7:
                            list_0.Add(binaryReader_0.ReadInt32());
                            break;
                        case 8:
                            list_0.Add(binaryReader_0.ReadInt64());
                            break;
                        case 9:
                            list_0.Add(binaryReader_0.ReadSByte());
                            break;
                        case 10:
                            list_0.Add(binaryReader_0.ReadInt16());
                            break;
                        case 11:
                            list_0.Add(binaryReader_0.ReadSingle());
                            break;
                        case 12:
                            list_0.Add(binaryReader_0.ReadString());
                            break;
                        case 13:
                            list_0.Add(binaryReader_0.ReadUInt32());
                            break;
                        case 14:
                            list_0.Add(binaryReader_0.ReadUInt64());
                            break;
                        case 15:
                            list_0.Add(binaryReader_0.ReadUInt16());
                            break;
                        case 16:
                            list_0.Add(DateTime.FromBinary(binaryReader_0.ReadInt64()));
                            break;
                        case 17:
                            {
                                string[] array2 = new string[binaryReader_0.ReadInt32() - 1 + 1];
                                int num2 = 0;
                                int num3 = array2.Length - 1;
                                for (int i = num2; i <= num3; i++)
                                {
                                    array2[i] = binaryReader_0.ReadString();
                                }
                                list_0.Add(array2);
                                break;
                            }
                        case 18:
                            {
                                List<object> list = list_0;
                                Guid guid = new Guid(binaryReader_0.ReadBytes(16));
                                list.Add(guid);
                                break;
                            }
                        case 19:
                            {
                                List<object> list2 = list_0;
                                Size size = new Size(binaryReader_0.ReadInt32(), binaryReader_0.ReadInt32());
                                list2.Add(size);
                                break;
                            }
                        case 20:
                            {
                                List<object> list3 = list_0;
                                Rectangle rectangle = new Rectangle(binaryReader_0.ReadInt32(), binaryReader_0.ReadInt32(), binaryReader_0.ReadInt32(), binaryReader_0.ReadInt32());
                                list3.Add(rectangle);
                                break;
                            }
                        case 21:
                            list_0.Add(new Version(binaryReader_0.ReadString()));
                            break;
                    }
                }
                gstruct.object_0 = list_0.ToArray();
                result = gstruct;
                list_0.Clear();
                binaryReader_0.Close();
            }
            return result;
        }
    }
}
