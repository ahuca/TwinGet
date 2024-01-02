// This file is licensed to you under MIT license.

namespace TwinGet.Utils.Test.IOTests
{
    internal static class DirectoryComparer
    {
        public static bool Equals(string path1, string path2)
        {
            DirectoryInfo dir1 = new(path1);
            DirectoryInfo dir2 = new(path2);

            IEnumerable<string> dir1SubDir = Directory.GetDirectories(dir1.FullName, "*", SearchOption.AllDirectories).Select(d => Path.GetRelativePath(dir1.FullName, d));
            IEnumerable<string> dir2SubDir = Directory.GetDirectories(dir2.FullName, "*", SearchOption.AllDirectories).Select(d => Path.GetRelativePath(dir2.FullName, d));
            bool sameDirectoryStructure = dir1SubDir.SequenceEqual(dir2SubDir);

            List<string> dir1Files = dir1.GetFiles("*.*", SearchOption.AllDirectories).Select(f => Path.GetRelativePath(dir1.FullName, f.FullName)).ToList();
            List<string> dir2Files = dir2.GetFiles("*.*", SearchOption.AllDirectories).Select(f => Path.GetRelativePath(dir2.FullName, f.FullName)).ToList();
            bool sameFileStructure = dir1Files.SequenceEqual(dir2Files);

            return sameDirectoryStructure && sameFileStructure;
        }
    }
}
