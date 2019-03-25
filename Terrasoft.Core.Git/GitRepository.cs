using LibGit2Sharp;

namespace Terrasoft.Core.Git
{
	public class GitRepository {
		void Clone(string sourceUrl, string workDirPath) {
			Repository.Clone(sourceUrl, workDirPath);
		}
	}
}
