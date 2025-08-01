using System.Collections.Generic;
using System.IO;
using System.Reflection;

public static class EmbedResourceLoader
{
	public static HashSet<string> ReadAsHashset(string resourceToLoad)
	{
		HashSet<string> returnValue = new HashSet<string>();

		Stream? namesStream = LoadResourceStream(resourceToLoad, typeof(EmbedResourceLoader).GetTypeInfo().Assembly);

		if (namesStream == null)
		{
			throw new FileNotFoundException($"Could not find resource with name: {resourceToLoad}");
		}

		using (StreamReader sr = new StreamReader(namesStream))
		{
			string? line;
			while ((line = sr.ReadLine()) != null)
			{
				if (line.StartsWith("//") || line.Length < 1)
				{
					continue;
				}

				returnValue.Add(line);
			}
		}

		return returnValue;
	}

	public static List<string> ReadAsList(string resourceToLoad)
	{
		List<string> returnValue = new List<string>();

		Stream? namesStream = LoadResourceStream(resourceToLoad, typeof(EmbedResourceLoader).GetTypeInfo().Assembly);
		
		if (namesStream == null)
		{
			throw new FileNotFoundException($"Could not find resource with name: {resourceToLoad}");
		}
		
		using (StreamReader sr = new StreamReader(namesStream))
		{
			string? line;
			while ((line = sr.ReadLine()) != null)
			{
				if (line.StartsWith("//") || line.Length < 1)
				{
					continue;
				}

				returnValue.Add(line);
			}
		}

		return returnValue;
	}

	private static Stream? LoadResourceStream(string resourceName, Assembly assembly)
	{
		string properResourceName = GetResourceName(assembly, resourceName);
		return assembly.GetManifestResourceStream(properResourceName);
	}

	private static string GetResourceName(Assembly assembly, string resourceName)
	{
		return $"{assembly.GetName().Name}.{resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".")}";
	}
}