using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace aclogview
{
	// TODO: Make Disposable
    class FragDatListFile
    {
        private static readonly byte FileVersion = 0x01;

        public enum CompressionType : byte
        {
            None            = 0x00,
            DeflateStream   = 0x01
        }

        public enum PacketDirection : byte
        {
            ServerToClient  = 0x00,
            ClientToServer  = 0x01
        }

        public class FragDatInfo
        {
            public PacketDirection PacketDirection;

            public int ReferenceIndex;

            public byte[] Data;

            public FragDatInfo(PacketDirection packetDirection, int referenceIndex, byte[] data)
            {
                PacketDirection = packetDirection;
                ReferenceIndex = referenceIndex;
                Data = data;
            }
        }


        private FileStream fileStream;
        private byte workingFileVersion;
        private CompressionType workingCompressionType;

        /// <summary>
        /// Will prompt the user for a path.<para />
        /// Use Write() to add data to the file, and CloseFile when you're finished.
        /// </summary>
        public bool CreateFile(CompressionType compressionType = CompressionType.DeflateStream)
        {
            string fileName;

            using (SaveFileDialog newDialog = new SaveFileDialog())
            {
                newDialog.Filter = "Frag Dat List (*.frags)|*.frags";
                newDialog.DefaultExt = ".frags";

                if (newDialog.ShowDialog() != DialogResult.OK)
                    return false;

                fileName = newDialog.FileName;

                if (String.IsNullOrEmpty(fileName))
                    return false;
            }

            return OpenFile(fileName);
        }

        /// <summary>
        /// Use Write() to add data to the file, and CloseFile when you're finished.<para />
        /// If the specified file already exists, it will be overwritten.
        /// </summary>
        public bool CreateFile(string fileName, CompressionType compressionType = CompressionType.DeflateStream)
        {
            fileStream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            workingFileVersion = FileVersion;
            workingCompressionType = compressionType;

            using (BinaryWriter fileWriter = new BinaryWriter(fileStream, System.Text.Encoding.Default, true))
            {
                fileWriter.Write(FileVersion);
                fileWriter.Write((byte)compressionType);
            }

            return fileStream != null;
        }

        /// <summary>
        /// Use CreateFile() first.<para /> 
        /// This function is not thread safe. 
        /// </summary>
        public bool Write(KeyValuePair<string, IList<FragDatInfo>> fragDatInfos)
        {
            if (fileStream == null)
                return false;


            // Setup the writer
            MemoryStream memoryStream = null; // Used if we're not writing ot the file directly
            BinaryWriter writer; // This will either write to the base fileStream, or to memoryStream

            if (workingCompressionType == CompressionType.None)
                writer = new BinaryWriter(fileStream, System.Text.Encoding.Default, true);
            else if (workingCompressionType == CompressionType.DeflateStream)
            {
                memoryStream = new MemoryStream();
                writer = new BinaryWriter(memoryStream, System.Text.Encoding.Default, true);
            }
            else
                throw new NotImplementedException();


            // Write
            writer.Write(fragDatInfos.Key ?? String.Empty);

            writer.Write(fragDatInfos.Value.Count);

            foreach (var fragDatInfo in fragDatInfos.Value)
            {
                writer.Write((byte)fragDatInfo.PacketDirection);
                writer.Write(fragDatInfo.ReferenceIndex);
                writer.Write((UInt16)fragDatInfo.Data.Length);
                writer.Write(fragDatInfo.Data);
            }


            // Transfer memoryStream to fileStream if we need to
            if (workingCompressionType == CompressionType.DeflateStream)
            {
                using (MemoryStream compressedMemoryStream = new MemoryStream())
                {
                    using (DeflateStream deflateStream = new DeflateStream(compressedMemoryStream, CompressionMode.Compress, true))
                        memoryStream.WriteTo(deflateStream);

                    if (compressedMemoryStream.Length > UInt32.MaxValue) throw new OverflowException();
                    fileStream.Write(new byte[] { (byte)(compressedMemoryStream.Length >> 0), (byte)(compressedMemoryStream.Length >> 8), (byte)(compressedMemoryStream.Length >> 16), (byte)(compressedMemoryStream.Length >> 24) }, 0, 4);
                    compressedMemoryStream.WriteTo(fileStream);
                }
            }


            // Close the writer
            writer.Dispose();

            if (memoryStream != null)
                memoryStream.Dispose();


            return true;
        }


        /// <summary>
        /// Will prompt the user for a path.<para />
        /// Use ReadNext to read data from the file, and CloseFile when you're finished.<para />  
        /// This will return null when the end of the file has been reached.
        /// </summary>
        public bool OpenFile()
        {
            string fileName;

            using (OpenFileDialog newDialog = new OpenFileDialog())
            {
                newDialog.Filter = "Frag Dat List (*.frags)|*.frags";
                newDialog.DefaultExt = ".frags";

                if (newDialog.ShowDialog() != DialogResult.OK)
                    return false;

                fileName = newDialog.FileName;

                if (String.IsNullOrEmpty(fileName))
                    return false;
            }

            return OpenFile(fileName);
        }

        /// <summary>
        /// Use ReadNext to read data from the file, and CloseFile when you're finished.<para />  
        /// This will return null when the end of the file has been reached.
        /// </summary>
        public bool OpenFile(string fileName)
        {
            fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            workingFileVersion = (byte)fileStream.ReadByte();
            workingCompressionType = (CompressionType)fileStream.ReadByte();

            return fileStream != null;
        }

        /// <summary>
        /// OpenFile() first.<para />
        /// This function is not thread safe.
        /// </summary>
        public bool TryReadNext(out KeyValuePair<string, List<FragDatInfo>> kvp)
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(fileStream, System.Text.Encoding.Default, true))
                {
                    if (workingFileVersion != 1)
                        throw new NotSupportedException();

                    string key;
                    var fragDataInfos = new List<FragDatInfo>();

                    if (workingCompressionType == CompressionType.None)
                    {
                        key = reader.ReadString();

                        var count = reader.ReadInt32();

                        for (int i = 0; i < count; i++)
                        {
                            var packetDirection = (PacketDirection)reader.ReadByte();

                            var referenceIndex = reader.ReadInt32();

                            var byteCount = reader.ReadUInt16();
                            var bytes = reader.ReadBytes(byteCount);

                            fragDataInfos.Add(new FragDatInfo(packetDirection, referenceIndex, bytes));
                        }
                    }
                    else if (workingCompressionType == CompressionType.DeflateStream)
                    {
                        int compressedDataSize = reader.ReadInt32();
                        byte[] compressedData = new byte[compressedDataSize];
                        reader.Read(compressedData, 0, compressedDataSize);

                        using (MemoryStream compressedDataStream = new MemoryStream(compressedData))
                        using (DeflateStream deflateStream = new DeflateStream(compressedDataStream, CompressionMode.Decompress))
                        using (BinaryReader deflateReader = new BinaryReader(deflateStream))
                        {
                            key = deflateReader.ReadString();

                            var count = deflateReader.ReadInt32();

                            for (int i = 0; i < count; i++)
                            {
                                var packetDirection = (PacketDirection)deflateReader.ReadByte();

                                var referenceIndex = deflateReader.ReadInt32();

                                var byteCount = deflateReader.ReadUInt16();
                                var bytes = deflateReader.ReadBytes(byteCount);

                                fragDataInfos.Add(new FragDatInfo(packetDirection, referenceIndex, bytes));
                            }
                        }
                    }
                    else
                        throw new NotImplementedException();

                    kvp = new KeyValuePair<string, List<FragDatInfo>>(key, fragDataInfos);
                    return true;
                }
            }
            catch
            {
                kvp = new KeyValuePair<string, List<FragDatInfo>>();
                return false;
            }
        }


        public void CloseFile()
        {
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream = null;
            }
        }
    }
}
