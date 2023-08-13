#include <string>
#include <fstream>

extern "C" {
	__declspec(dllexport) int CheckFiles(const char* file1Path, const char* file2Path);
}

int CheckFiles(const char* file1Path, const char* file2Path) {
	std::ifstream file1(file1Path);
	std::ifstream file2(file2Path);
	std::string content1, content2;

	std::getline(file1, content1);
	std::getline(file2, content2);

	file1.close();
	file2.close();

	if (content1 == "V2" && content2 == "V1")
		return 1;
	else if (content2 == "V2" && content1 == "V1")
		return 2;

	return 0;
}