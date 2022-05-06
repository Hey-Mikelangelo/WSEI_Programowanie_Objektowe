using System;
using System.Collections.Generic;
    using System.IO;
using System.Text;
using static lab_4.FileSizeFormatter;

namespace lab_4
{
    public class DirectoryScanner
    {
        private static readonly string
            _otherTypeLabel = "[other]",
            _countLabel = "[count]",
            _totalSizeLabel = "[totalSize]",
            _avarageSizeLabel = "[avgSize]",
             _minSizeLabel = "[minSize]",
            _maxSizeLabel = "[maxSize]",
            _pathLabel = "[path]",
            _sizeLabel = "[size]";

        private const int
            _col0Alignment = -20,
            _col1Alignment = -10,
            _col2Alignment = -13,
            _col3Alignment = -13,
            _col4Alignment = -13,
            _col5Alignment = -13,
            _col6Alignment = -13;

        private static readonly List<string> _defaultScannedFileExtensions = new List<string>
            {
               "jpg", "png", "gif",
                "doc", "txt", "mp3"
            };

        public void Scan(string scannedDirectoryPath, int maxFilesToShowCount = int.MaxValue, bool showAllExtensions = false, 
            List<string> fileExtensionsToShow = null)
        {
            if (Directory.Exists(scannedDirectoryPath))
            {
                Console.WriteLine($"\n\"Scanning {scannedDirectoryPath}\" ...\n");
            }
            else
            {
                Console.WriteLine($"Path \"{scannedDirectoryPath}\" does not exists");
                return;
            }
            var subDirectoriesPaths = Directory.GetDirectories(scannedDirectoryPath);
            int directoriesCount = subDirectoriesPaths.Length;

            List<File> scannedDirectoryFiles = GetFiles(scannedDirectoryPath);
            List<File> sudbdrectoryFiles = GetSubDirectoriesFiles(subDirectoriesPaths, directoriesCount);

            int scannedDirectoryFilesCount = scannedDirectoryFiles.Count;

            List<File> allFiles = new List<File>(scannedDirectoryFiles);
            allFiles.AddRange(sudbdrectoryFiles);

            SortedSet<File> sortedByNameFiles, sortedBySizeFiles;
            Dictionary<string, List<File>> extensionToFilesDict;
            long scannedDirectoryFilesSize, subDirectoriesFilesSize;

            GetSortedData(scannedDirectoryFilesCount, allFiles,
                out sortedByNameFiles,
                out sortedBySizeFiles,
                out extensionToFilesDict,
                out scannedDirectoryFilesSize,
                out subDirectoriesFilesSize);
            int filesCount = allFiles.Count;

            long filesSize = subDirectoriesFilesSize + scannedDirectoryFilesSize;

            List<FilesGroupStatistics> filesStatsByExtensions = GetFilesStatisticsByExtensions(extensionToFilesDict);
            List<FilesGroupStatistics> filesStatsByTypes = GetFilesStatisticsByTypes(filesStatsByExtensions);
            List<FilesGroupStatistics> filesStatsBySizes = GetFilesStatisticsBySizes(sortedBySizeFiles);
            List<KeyValuePair<char, int>> filesCountsByFirstLetter = GetFirstLettersWithCounts(sortedByNameFiles);

            string directoryNodesStatsText = GetDirectoryNodesStatsText(
                directoriesCount, subDirectoriesFilesSize, filesCount, filesSize, 1);

            string filesStatsByTypesText = GetFilesStatsByTypesText(filesStatsByTypes, 2);

            string filesStatsByExtensionsText = GetFilesStatsByExtensionsText(
                    filesStatsByExtensions, showAllExtensions, leadingTabsCount: 2);
          
            string filesStatsBySizeText = GetFilesStatsBySizeText(filesStatsBySizes, 2);

            string filesCountsByFirstLetterText = GetFilesCountsByFirstLetterText(filesCountsByFirstLetter, 1);

            int maxItemsToShowCount = showAllExtensions ? maxFilesToShowCount : 20;
            string filesOrderedByName = GetFilesOrderedByNameText(sortedByNameFiles, maxItemsToShow: maxItemsToShowCount, 1);

            string filesOrderedBySize = GetFilesOrderedBySizeText(sortedBySizeFiles, maxItemsToShow: maxItemsToShowCount, 1);

            Console.WriteLine("Nodes:");
            Console.Write(directoryNodesStatsText);
            Console.WriteLine("\nFiles:\n\tBy types:");
            Console.Write(filesStatsByTypesText);
            Console.WriteLine("\n\tBy extensions:");
            Console.Write(filesStatsByExtensionsText);
            Console.WriteLine("\n\tBy sizes:");
            Console.Write(filesStatsBySizeText);
            Console.WriteLine("\n\tCounts by first leter:");
            Console.Write(filesCountsByFirstLetterText);
            Console.WriteLine("\n\tOrdered by name:");
            Console.Write(filesOrderedByName);
            Console.WriteLine("\n\tOrdered by sizes (from biggest):");
            Console.Write(filesOrderedBySize);
        }

        private List<File> GetFiles(string directoryPath)
        {
            var filesPaths = Directory.GetFiles(directoryPath);
            int filesInDirectoryCount = filesPaths.Length;
            var files = new List<File>(filesInDirectoryCount);
            for (int i = 0; i < filesInDirectoryCount; i++)
            {
                string filePath = filesPaths[i];
                files.Add(new FileInfo(filePath));
            }
            return files;
        }

        private List<File> GetFilesRecursive(string directoryPath)
        {
            string[] subDirectoriesPaths;
            try
            {
                subDirectoriesPaths =  Directory.GetDirectories(directoryPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<File>();
            }

            int subDirectoriesCount = subDirectoriesPaths.Length;

            var files = GetFiles(directoryPath);
            for (int i = 0; i < subDirectoriesCount; i++)
            {
                string subDirectoryPath = subDirectoriesPaths[i];
                var subDirectoryFiles = GetFilesRecursive(subDirectoryPath);
                files.AddRange(subDirectoryFiles);
            }
            return files;
        }

        private List<File> GetSubDirectoriesFiles(string[] subDirectoriesPaths, int directoriesCount)
        {
            List<File> sudbdrectoriesFiles = new List<File>(0);
            for (int i = 0; i < directoriesCount; i++)
            {
                sudbdrectoriesFiles.AddRange(GetFilesRecursive(subDirectoriesPaths[i]));
            }

            return sudbdrectoriesFiles;
        }

        private static void GetSortedData(int scannedDirectoryFilesCount, List<File> files,
            out SortedSet<File> sortedByNameFiles, out SortedSet<File> sortedBySizeFiles,
            out Dictionary<string, List<File>> extensionToFilesDict,
            out long scannedDirectoryFilesSize, out long subDirectoriesFilesSize)
        {
            //Using sorted set for fast splitting set by size ranges
            sortedBySizeFiles = new SortedSet<File>(new FileSizeComparer(fromBiggest: true));
            //Using sorted set for counting first files letters and displaying sorted files by names
            sortedByNameFiles = new SortedSet<File>(new FileNameComparer());
            //using dictionary to count files by extensions statistics using stored in dictionary list
            extensionToFilesDict = new Dictionary<string, List<File>>();
            scannedDirectoryFilesSize = 0;
            subDirectoriesFilesSize = 0;
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];

                if (i < scannedDirectoryFilesCount)
                {
                    scannedDirectoryFilesSize += file.lenght;
                }
                else
                {
                    subDirectoriesFilesSize += file.lenght;

                }

                sortedBySizeFiles.Add(file);

                sortedByNameFiles.Add(file);

                string extension = file.extension;
                if (extensionToFilesDict.ContainsKey(extension) == false)
                {
                    extensionToFilesDict.Add(extension, new List<File>() { file });
                }
                else
                {
                    var extensionsFiles = extensionToFilesDict[extension];
                    extensionsFiles.Add(file);
                }
            }
        }

        private List<FilesGroupStatistics> GetFilesStatisticsByExtensions(Dictionary<string, List<File>> extensionToFilesDict)
        {
            var extensions = extensionToFilesDict.Keys;
            var filesStatisticsByExtension = new List<FilesGroupStatistics>();

            foreach (var extension in extensions)
            {
                List<File> files = extensionToFilesDict[extension];
                string extensionGroupName = extension.Trim('.');
                filesStatisticsByExtension.Add(GetFilesGroupStatistics(files, extensionGroupName));
            }

            return filesStatisticsByExtension;
        }

        private List<FilesGroupStatistics> GetFilesStatisticsByTypes(List<FilesGroupStatistics> filesStatisticsByExtensions)
        {
            int fileTypesCount = 5;
            Span<FileType> scannedFileTypes = stackalloc FileType[fileTypesCount];
            scannedFileTypes[0] = FileType.Image;
            scannedFileTypes[1] = FileType.Audio;
            scannedFileTypes[2] = FileType.Video;
            scannedFileTypes[3] = FileType.Document;
            scannedFileTypes[4] = FileType.Other;

            List<FilesGroupStatistics> filesStatisticsByTypes = new List<FilesGroupStatistics>(fileTypesCount);

            for (int i = 0; i < fileTypesCount - 1; i++)
            {
                var statistics = new FilesGroupStatistics((scannedFileTypes[i].ToString()).ToLower());
                filesStatisticsByTypes.Add(statistics);
            }
            var otherStatistics = new FilesGroupStatistics(_otherTypeLabel);
            filesStatisticsByTypes.Add(otherStatistics);

            for (int i = 0; i < filesStatisticsByExtensions.Count; i++)
            {
                var extensionFilesStatistics = filesStatisticsByExtensions[i];
                FileType filesType;
                if (FileTypesData.extensionFileType.TryGetValue(extensionFilesStatistics.groupName, out var type))
                {
                    filesType = type;
                }
                else
                {
                    filesType = FileType.Other;
                }
                int index = GetFileTypeInListIndex(scannedFileTypes, filesType);
                FilesGroupStatistics stats = filesStatisticsByTypes[index];
                stats.Merge(extensionFilesStatistics);
                filesStatisticsByTypes[index] = stats;
            }

            for (int i = 0; i < fileTypesCount; i++)
            {
                var stats = filesStatisticsByTypes[i];
                stats = stats.GetStatistics();
                filesStatisticsByTypes[i] = stats;
            }

            return filesStatisticsByTypes;

            int GetFileTypeInListIndex(Span<FileType> fileTypes, FileType fileType)
            {
                for (int i = 0; i < fileTypes.Length; i++)
                {
                    if (fileTypes[i] == fileType)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        private List<FilesGroupStatistics> GetFilesStatisticsBySizes(SortedSet<File> filesBySizesAsc)
        {
            int groupsCount = 4;
            var filesStatisticsBySizes = new List<FilesGroupStatistics>(groupsCount);
            Span<MinMax> sizeGroups = stackalloc MinMax[groupsCount];

            sizeGroups[0] = new MinMax() { Min = 0, Max = 1024 };
            sizeGroups[1] = new MinMax() { Min = 1024 + 1, Max = 1048576 };
            sizeGroups[2] = new MinMax() { Min = 1048576 + 1, Max = 1073741824 };
            sizeGroups[3] = new MinMax() { Min = 1073741824 + 1, Max = long.MaxValue };

            for (int i = 0; i < sizeGroups.Length; i++)
            {
                File minValue = new File(sizeGroups[i].Min);
                File maxValue = new File(sizeGroups[i].Max);
                SortedSet<File> filesGroup = filesBySizesAsc.GetViewBetween(maxValue, minValue);

                filesStatisticsBySizes.Add(GetFilesGroupStatistics(filesGroup, GetFileGroupName(i)));
            }

            return filesStatisticsBySizes;

            string GetFileGroupName(int index)
            {
                switch (index)
                {
                    case 0: return "      . <= 1KB";
                    case 1: return "1KB < . <= 1MB";
                    case 2: return "1MB < . <= 1GB";
                    case 3: return "1GB < .       ";
                }
                return "Not valid group";
            }

        }

        private static List<KeyValuePair<char, int>> GetFirstLettersWithCounts(SortedSet<File> sortedByNameFiles)
        {
            var firstLettersWithCounts = new List<KeyValuePair<char, int>>();
            char currentChar = '\0';
            int currentCharsCount = 0;
            foreach (var file in sortedByNameFiles)
            {
                var nameChars = file.name.ToCharArray();

                var currentLetter = char.ToUpper(nameChars[0]);
                if (currentLetter == currentChar)
                {
                    currentCharsCount++;
                }
                else
                {
                    if (currentCharsCount > 0)
                    {
                        AddLetterWithCount(currentChar, currentCharsCount);
                    }
                    currentCharsCount = 0;

                    currentChar = currentLetter;
                    currentCharsCount++;
                }

            }
            AddLetterWithCount(currentChar, currentCharsCount);



            void AddLetterWithCount(char letter, int count)
            {
                firstLettersWithCounts.Add(new KeyValuePair<char, int>(letter, count));
            }

            return firstLettersWithCounts;
        }


        private FilesGroupStatistics GetFilesGroupStatistics(IEnumerable<File> files, string groupName)
        {
            FilesGroupStatistics groupStatistics = default;

            groupStatistics.Init();

            foreach (var file in files)
            {
                groupStatistics.Add(file);
            }
            groupStatistics.groupName = groupName;
            return groupStatistics.GetStatistics();
        }

        private struct MinMax
        {
            public long Min, Max;
        }

        private struct File
        {
            public long lenght;
            public string name;
            public string path;
            public string extension;

            public File(long length)
            {
                lenght = length;
                name = string.Empty;
                path = string.Empty;
                extension = string.Empty;
            }
            public File(FileInfo fileInfo)
            {
                lenght = fileInfo.Length;
                name = fileInfo.Name;
                path = fileInfo.FullName;
                extension = fileInfo.Extension;
            }

            public static implicit operator File(FileInfo fileInfo)
            {
                return new File(fileInfo);
            }
        }

        private struct FilesGroupStatistics
        {
            public string groupName;
            public int count;
            public long totalSize;
            public long minSize;
            public long maxSize;

            public FilesGroupStatistics(string groupName)
            {
                this.groupName = groupName;
                count = 0;
                totalSize = 0;
                minSize = long.MaxValue;
                maxSize = long.MinValue;
            }

            public FilesGroupStatistics GetStatistics()
            {
                FilesGroupStatistics statistics = this;
                if (statistics.totalSize == 0)
                {
                    statistics.minSize = 0;
                    statistics.maxSize = 0;
                }
                return statistics;
            }
            public void Init()
            {
                groupName = "Unassigned group name";
                count = 0;
                totalSize = 0;
                minSize = long.MaxValue;
                maxSize = long.MinValue;
            }

            public override string ToString()
            {
                float avgSize = count != 0 ? (float)totalSize / count : 0;
                return
                    $"{groupName,_col0Alignment}" +
                    $"{count,_col1Alignment}" +
                    $"{FormatSize(totalSize),_col2Alignment}" +
                    $"{FormatSize((long)avgSize),_col3Alignment}" +
                    $"{FormatSize(minSize),_col4Alignment}" +
                    $"{FormatSize(maxSize),_col5Alignment}";
            }

            public void Add(File file)
            {
                long size = file.lenght;
                count = count + 1;
                totalSize = totalSize + size;
                if (size > maxSize)
                {
                    maxSize = size;
                }
                if (size < minSize)
                {
                    minSize = size;
                }
            }
            public void Merge(FilesGroupStatistics other)
            {
                count += other.count;
                totalSize += other.totalSize;
                minSize = Math.Min(minSize, other.minSize);
                maxSize = Math.Max(maxSize, other.maxSize);
            }
        }

        private string GetDirectoryNodesStatsText(int directoriesCount, long directoriesTotalSize,
    int filesCount, long filesTotalSize, int leadingTabsCount = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string row0 = GetRow(leadingTabsCount, string.Empty, _countLabel, _totalSizeLabel);
            string row1 = GetRow(leadingTabsCount, "Directories:", directoriesCount.ToString(), FormatSize(directoriesTotalSize));
            string row2 = GetRow(leadingTabsCount, "Files:", filesCount.ToString(), FormatSize(filesTotalSize));

            stringBuilder.Append(row0);
            stringBuilder.Append(row1);
            stringBuilder.Append(row2);

            return stringBuilder.ToString();
        }

        private string GetFilesStatsByTypesText(List<FilesGroupStatistics> filesStatsByTypes, int leadingTabsCount = 0)
        {
            return GetFileStatisticsRows(filesStatsByTypes, leadingTabsCount);
        }

        private string GetFilesStatsByExtensionsText(List<FilesGroupStatistics> filesStatsByExtensions, bool listAllExtensions,
            List<string> scannedFileExtensions = null, int leadingTabsCount = 0)
        {
            if (scannedFileExtensions == null && listAllExtensions == false)
            {
                scannedFileExtensions = _defaultScannedFileExtensions;
            }
            string filesStatsRows;
            if (listAllExtensions)
            {
                filesStatsRows = GetFileStatisticsRows(filesStatsByExtensions, leadingTabsCount);
            }
            else
            {
                var otherGroupsStatistics = new FilesGroupStatistics(_otherTypeLabel);
                List<FilesGroupStatistics> filesStatsByExtensionsCut = new List<FilesGroupStatistics>(scannedFileExtensions.Count + 1);
                foreach (var filesStatistics in filesStatsByExtensions)
                {
                    if (scannedFileExtensions.Contains(filesStatistics.groupName) == false)
                    {
                        otherGroupsStatistics.Merge(filesStatistics);
                    }
                    else
                    {
                        filesStatsByExtensionsCut.Add(filesStatistics);
                    }
                }
                filesStatsByExtensionsCut.Add(otherGroupsStatistics);
                filesStatsRows = GetFileStatisticsRows(filesStatsByExtensionsCut, leadingTabsCount);
            }

            return filesStatsRows;
        }

        private string GetFilesStatsBySizeText(List<FilesGroupStatistics> filesStatsBySize, int leadingTabsCount = 0)
        {
            return GetFileStatisticsRows(filesStatsBySize, leadingTabsCount);
        }

        private string GetFilesCountsByFirstLetterText(List<KeyValuePair<char, int>> fileCountsByFirstLetter,
            int leadingTabsCount = 0)
        {
            StringBuilder stringBuilderLetters = new StringBuilder();
            StringBuilder stringBuilderCounts = new StringBuilder();
            foreach (var item in fileCountsByFirstLetter)
            {
                stringBuilderCounts.Append('\t', leadingTabsCount);
                stringBuilderLetters.Append('\t', leadingTabsCount);

                stringBuilderCounts.Append($"{item.Value,-4}");
                int additionalSpacesCounter = item.Value;

                stringBuilderLetters.Append($"{item.Key,-4}");

                while (additionalSpacesCounter > 999)
                {
                    stringBuilderLetters.Append($" ");
                    stringBuilderCounts.Append($" ");
                    additionalSpacesCounter /= 10;
                }
            }
            stringBuilderLetters.Append('\n');
            stringBuilderLetters.Append(stringBuilderCounts);
            stringBuilderLetters.Append('\n');

            return stringBuilderLetters.ToString();
        }

        private string GetFilesOrderedByNameText(SortedSet<File> filesOrderedByName, int maxItemsToShow = 20,
            int leadingTabsCount = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string labelRow = GetRowLong(leadingTabsCount, string.Empty, _sizeLabel ,_pathLabel);
            stringBuilder.Append(labelRow);
            int i = 0;
            foreach (var item in filesOrderedByName)
            {
                if (i > maxItemsToShow) break;
                string row = GetRowLong(leadingTabsCount, item.name, FormatSize(item.lenght), item.path);
                stringBuilder.Append(row);
                i++;
            }
            return stringBuilder.ToString();
        }

        private string GetFilesOrderedBySizeText(SortedSet<File> filesOrderedBySize, int maxItemsToShow = 20,
            int leadingTabsCount = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string labelRow = GetRowLong(leadingTabsCount, string.Empty, _sizeLabel, _pathLabel);
            stringBuilder.Append(labelRow);
            int i = 0;
            foreach (var item in filesOrderedBySize)
            {
                if (i > maxItemsToShow) break;
                string row = GetRowLong(leadingTabsCount, item.name, FormatSize(item.lenght), item.path);
                stringBuilder.Append(row);
                i++;

            }
            return stringBuilder.ToString();

        }
        private string GetFileStatisticsRows(IEnumerable<FilesGroupStatistics> filesStats, int leadingTabsCount)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string labelsRow = GetRow(leadingTabsCount,
                string.Empty, _countLabel, _totalSizeLabel, _avarageSizeLabel, _minSizeLabel, _maxSizeLabel);
            stringBuilder.Append(labelsRow);
            foreach (var item in filesStats)
            {
                stringBuilder.Append('\t', leadingTabsCount);
                stringBuilder.Append(item.ToString());
                stringBuilder.Append('\n');
            }
            return stringBuilder.ToString();
        }
        private string GetRowLong(int leadingTabsCount, params string[] entries)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('\t', leadingTabsCount);
            for (int i = 0; i < entries.Length; i++)
            {
                string entry = entries[i];
                switch (i)
                {
                    case 0: stringBuilder.Append($"{entry,_col0Alignment - 20 }"); break;
                    case 1: stringBuilder.Append($"{entry,_col1Alignment}"); break;
                    case 2: stringBuilder.Append($"{entry,_col2Alignment - 20}"); break;
                    case 3: stringBuilder.Append($"{entry,_col3Alignment}"); break;
                    case 4: stringBuilder.Append($"{entry,_col4Alignment}"); break;
                    case 5: stringBuilder.Append($"{entry,_col5Alignment}"); break;
                    case 6: stringBuilder.Append($"{entry,_col6Alignment}"); break;
                }
            }
            stringBuilder.Append('\n');
            return stringBuilder.ToString();
        }
        private string GetRow(int leadingTabsCount, params string[] entries)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('\t', leadingTabsCount);
            for (int i = 0; i < entries.Length; i++)
            {
                string entry = entries[i];
                switch (i)
                {
                    case 0: stringBuilder.Append($"{entry,_col0Alignment}"); break;
                    case 1: stringBuilder.Append($"{entry,_col1Alignment}"); break;
                    case 2: stringBuilder.Append($"{entry,_col2Alignment}"); break;
                    case 3: stringBuilder.Append($"{entry,_col3Alignment}"); break;
                    case 4: stringBuilder.Append($"{entry,_col4Alignment}"); break;
                    case 5: stringBuilder.Append($"{entry,_col5Alignment}"); break;
                    case 6: stringBuilder.Append($"{entry,_col6Alignment}"); break;
                }
            }
            stringBuilder.Append('\n');
            return stringBuilder.ToString();
        }

        private class FileSizeComparer : IComparer<File>
        {
            private bool _fromBiggest;

            public FileSizeComparer(bool fromBiggest = false)
            {
                _fromBiggest = fromBiggest;
            }
            public int Compare(File x, File y)
            {
                return _fromBiggest ? y.lenght.CompareTo(x.lenght) : x.lenght.CompareTo(y.lenght);
            }
        }

        private class FileNameComparer : IComparer<File>
        {
            private bool _fromZToA;

            public FileNameComparer(bool fromZToA = false)
            {
                _fromZToA = fromZToA;
            }

            public int Compare(File x, File y)
            {
                return _fromZToA ? y.name.CompareTo(x.name) : x.name.CompareTo(y.name);
            }
        }


    }


}
