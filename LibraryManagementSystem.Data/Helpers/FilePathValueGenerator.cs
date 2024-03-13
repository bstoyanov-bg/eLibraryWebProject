using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

public class FilePathValueGenerator : ValueGenerator<string>
{
    public override string Next(EntityEntry entry)
    {
        var bookTitle = entry.Entity.GetType().GetProperty("Title")?.GetValue(entry.Entity)?.ToString();
        var fileName = $"{bookTitle ?? "default"}_file.txt";
        return Path.Combine("BookFiles", "Books", fileName);
    }

    public override bool GeneratesTemporaryValues => false;
}