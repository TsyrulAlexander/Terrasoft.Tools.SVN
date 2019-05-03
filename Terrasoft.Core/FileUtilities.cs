using System.IO;
using System.IO.Compression;

namespace Terrasoft.Core
{
    public static class FileUtilities
    {
        public static void UnZip(byte[] file, string directory)
        {
            using (var zippedStream = new MemoryStream(file)) {
                using (var archive = new ZipArchive(zippedStream)) {
                    foreach (ZipArchiveEntry zipArchiveEntry in archive.Entries) {
                        byte[] entryFile = GetArchiveEntryToFile(zipArchiveEntry);
                        Directory.CreateDirectory(directory);
                        Save(entryFile, directory + @"\" + zipArchiveEntry.Name);
                    }
                }
            }
        }

        private static byte[] GetArchiveEntryToFile(ZipArchiveEntry archiveEntry)
        {
            using (Stream unzippedEntryStream = archiveEntry.Open()) {
                using (var ms = new MemoryStream()) {
                    unzippedEntryStream.CopyTo(ms);
                    byte[] unzippedArray = ms.ToArray();
                    return unzippedArray;
                }
            }
        }

        public static void Save(byte[] file, string path)
        {
            File.WriteAllBytes(path, file);
        }
    }
}