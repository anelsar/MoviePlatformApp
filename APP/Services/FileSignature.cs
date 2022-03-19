using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace APP.Services
{
	public class FileSignature
	{
		private static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
		{
		{ ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
		{ ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
		{ ".jpeg", new List<byte[]>
			{
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
			}
		},
		{ ".jpg", new List<byte[]>
			{
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
				new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
			}
		},
		
		};

		// checking the file signature
		public bool IsValidSignature(string fileExtension, Stream data)
        {
			data.Position = 0;
			using (var reader = new BinaryReader(data))
            {
				var signatures = _fileSignature[fileExtension];
				var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

				return signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }



	}
}