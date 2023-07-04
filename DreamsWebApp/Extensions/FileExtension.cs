namespace DreamsWebApp.Extensions;
public static class FileExtension
{
	public static bool CheckType(this IFormFile file, string type)
	{
		return file.ContentType.Contains(type);
	}

	public static bool CheckSize(this IFormFile file, double kb)
	{
		return file.Length / 1024 > kb;
	}

    public static async Task<string> UplaodAsync(this IFormFile file, params string[] folders)
	{
		string newFilename = Guid.NewGuid().ToString() + file.FileName;
		string FoldersPath = Path.Combine(folders);
		string path = Path.Combine(FoldersPath, newFilename);

		using (FileStream fileStream = new FileStream(path, FileMode.CreateNew))
		{
			await file.CopyToAsync(fileStream);
		}
		return newFilename;
	}
}