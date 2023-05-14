using System;
using System.Data;
using System.IO;

/*
    Design Goals
    * No Exceptions, provide Error messages instead - Allows for high performance code

*/

namespace SharpIconLib
{
    public enum SharpImageType
    {
        Icon = 1,
        Cursor = 2,
        Invalid = ushort.MaxValue
    }

    public struct SharpIconHeader
    {
        public readonly ushort Reserved { get; }
        public readonly ushort ImageTypeValue { get; }
        public readonly SharpImageType get => GetEnumImageType(ImageTypeValue);
        public readonly ushort ImageCount { get; }

        private static SharpImageType GetEnumImageType(ushort imageTypeValue)
        {
            if (imageTypeValue > 2 || imageTypeValue == 0)
                return SharpImageType.Invalid;

            if (imageTypeValue == 1)
                return SharpImageType.Icon;

            if (imageTypeValue == 2)
                return SharpImageType.Cursor;

            // This should actually be impossible, since all possible values should be covered above
            throw new NotImplementedException();
        }
    }

    public struct SharpDirectoryEntry
    {
    }
    public struct SharpIconData
    {

    }

    public class SharpIcon
    {
        public static void Load(Stream imageStream)
        {
            if (imageStream == null)
                throw new ArgumentNullException("Given stream may not be null", nameof(imageStream));

            if (!imageStream.CanRead)
                throw new InvalidOperationException("Cannot read data from given stream. CanRead property of given stream is false");

            MemoryStream memoryStream = new MemoryStream();
            imageStream.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            if (memoryStream.Length > int.MaxValue)
            {
                throw new InvalidOperationException($"Reading images bigger than {int.MaxValue} bytes is not supported");
            }

            using BinaryReader reader = new BinaryReader(memoryStream);
            byte[] imageData = reader.ReadBytes((int)memoryStream.Length);

            if (imageData.Length < 6)
            {
                // return error, that imageheader/filesize is too short
            }

            UInt16 reserved = GetUInt16(imageData, 0);
            UInt16 imageType = GetUInt16(imageData, 2);
            UInt16 imageCount = GetUInt16(imageData, 4);

            int currentOffset = 6;
            for (int i = 0; i < imageCount; i++)
            {
                // Somehow we need to check, that imageData is long enough to get the data
                byte imageWidth = imageData[currentOffset];
                currentOffset++;

                byte imageHeight = imageData[currentOffset];
                currentOffset++;

                byte paletteColorCount = imageData[currentOffset];
                currentOffset++;

                byte dirEntryReserved = imageData[currentOffset];
                currentOffset++;

                UInt16 planesOrHorizontal = GetUInt16(imageData, currentOffset);
                currentOffset += 2;

                UInt16 pixelBitsOrVertical = GetUInt16(imageData, currentOffset);
                currentOffset += 2;
                
                UInt32 imageDataLength = GetUInt32(imageData, currentOffset);
                currentOffset += 4;

                UInt32 imageDataOffset = GetUInt32(imageData, currentOffset);
            }
        }

        private static UInt16 GetUInt16(byte[] data, int offset)
        {
            byte[] twoBytes = new byte[2];
            Array.Copy(data, offset, twoBytes, 0, 2);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(twoBytes);
            }

            UInt16 result = BitConverter.ToUInt16(twoBytes);
            return result;
        }

        private static UInt32 GetUInt32(byte[] data, int offset)
        {
            byte[] fourBytes = new byte[4];
            Array.Copy(data, offset, fourBytes, 0, 4);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(fourBytes);
            }

            UInt32 result = BitConverter.ToUInt32(fourBytes);
            return result;
        }
    }
}
