var lines = File.ReadAllLines(@"input.txt");

var root = new DirectoryEntry("/", null);
var allDirectories = new List<DirectoryEntry>();

var currentDirectory = root;
foreach (var line in lines.Skip(2))
{
    var split = line.Split(" ");

    if (split[0] == "$")
    {
        if (split[1] == "ls")
        {
            continue;
        }

        if (split[1] == "cd")
        {
            if (split[2] == "..")
            {
                currentDirectory = currentDirectory.Parent;
            }
            else
            {
                currentDirectory = currentDirectory.ChildEntries.First(e => e.Name == split[2]) as DirectoryEntry;
            }
        }
    }
    else
    {
        if (split[0] == "dir")
        {
            allDirectories.Add(currentDirectory.AddDirectory(split[1]));
        }
        else
        {
            currentDirectory.AddFile(split[1], long.Parse(split[0]));
        }        
    }
}

var result1 = allDirectories.Where(d => d.CalculateSize() <= 100_000).Sum(d => d.CalculateSize());
Console.WriteLine(result1);

var total = 70_000_000L;
var target = 30_000_000L;
var current = total - root.CalculateSize();
var needed = target - current;

var result2 = allDirectories.Where(d => d.CalculateSize() >= needed).Min(d => d.CalculateSize());
Console.WriteLine(result2);


public abstract class FilesystemEntry
{
    protected FilesystemEntry(string name, DirectoryEntry? parent)
    {
        Name = name;
        Parent = parent;
    }

    public DirectoryEntry? Parent { get; }

    public string Name { get; }

    public abstract long CalculateSize();
}

public sealed class DirectoryEntry : FilesystemEntry
{
    public DirectoryEntry(string name, DirectoryEntry? parent) : base(name, parent)
    {
        ChildEntries = new List<FilesystemEntry>();
    }
    
    public List<FilesystemEntry> ChildEntries { get; }

    public override long CalculateSize()
    {
        return ChildEntries.Sum(entry => entry.CalculateSize());
    }

    public DirectoryEntry AddDirectory(string name)
    {
        var directory = new DirectoryEntry(name, this);
        ChildEntries.Add(directory);

        return directory;
    }

    public FileEntry AddFile(string name, long size)
    {
        var file = new FileEntry(name, this, size);
        ChildEntries.Add(file);

        return file;
    }
}

public sealed class FileEntry : FilesystemEntry
{
    public FileEntry(string name, DirectoryEntry? parent, long size) : base(name, parent)
    {
        Size = size;
    }

    public long Size { get; }

    public override long CalculateSize()
    {
        return Size;
    }
}