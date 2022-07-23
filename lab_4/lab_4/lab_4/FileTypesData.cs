using System;
using System.Collections.Generic;
using static lab_4.FileExtensions;

namespace lab_4
{
    public static class FileTypesData
    {

        public static readonly Dictionary<FileType, string[]> fileTypeExtensions = new Dictionary<FileType, string[]>()
        {
            { FileType.Image, new string[] { png, webm, jpg, gif, tiff } },
            { FileType.Audio, new string[] { ogg, mp3 } },
            { FileType.Video, new string[] { mkv, mp4 , webm } },
            { FileType.Document, new string[] { txt, doc, docx, xml, xlmx } }
        };

        public static readonly Dictionary<string, FileType> extensionFileType = GetValueToKeyDict(fileTypeExtensions);

        public static Dictionary<V, K> GetValueToKeyDict<K, V>(Dictionary<K, V[]> dict)
        {
            IEnumerable<K> keys = dict.Keys;

            Dictionary<V, K> valueToKeyPairs = new Dictionary<V, K>();
            foreach (var key in keys)
            {
                var values = dict[key];
                foreach (var value in values)
                {
                    if(valueToKeyPairs.ContainsKey(value) == false)
                    {
                        valueToKeyPairs.Add(value, key);
                    }
                }
            };
            return valueToKeyPairs;
        }
    }
}
