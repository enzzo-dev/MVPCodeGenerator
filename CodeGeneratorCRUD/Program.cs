using System.Data.Common;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;

public class Program
{
    static void Main()
    {
        GenerateApplication.InitializeConsole();
        var generationFile = new ClassGeneratorFile();

        generationFile.InitializeProgram();

    }

}



public class ClassGeneratorFile
{
    public void InitializeProgram(){
        const int MVC_PATTERN = 1;
        const int CQRS_PATTERN = 2;

        int designPattern = 0;

        do {

            Console.WriteLine("Que tipo de padrão você deseja criar? (1 - MVC, 2 - CQRS)");
            if(int.TryParse(Console.ReadLine().ToString(), out var result))
                designPattern = result;
         
        } while(designPattern <= 0);

        var (solutionPath, folderSolution) = CreateDirectory();

        switch(designPattern){
            case MVC_PATTERN:
                GenerateMVCStructure(solutionPath, folderSolution);
                break;
            case CQRS_PATTERN:
                GenerateCQRSPattern(solutionPath, folderSolution);
                break;      
        }

    }

    private void GenerateMVCStructure(string solutionPath, string folderSolution){
        try
        {
            var appPath = Path.Combine(solutionPath, folderSolution);

            if(!Directory.Exists(appPath))
                Directory.CreateDirectory(appPath);
         
            Console.WriteLine("Qual o nome da solução?");
            var nameSolution = Console.ReadLine();

            Directory.SetCurrentDirectory(appPath);

            var files = Directory.GetFiles(appPath);

            if(!files.Contains($"{Path.Combine(appPath, nameSolution)}.csproj"))
            {
                Process.Start("dotnet", $"new sln -n {nameSolution} -o {appPath}").WaitForExit();
                Process.Start("dotnet", $"new mvc -n {nameSolution} -o {appPath}").WaitForExit();   
            } else{
                Console.WriteLine($"Aplicação com o mesmo nome e na mesma pasta já existe: {appPath}");
            }
            
        }catch(Exception ex)
        {
            Console.WriteLine("Ocorreu um erro ao criar a nova pasta: " + ex.Message);     
        }

    }

    private void GenerateCQRSPattern(string solutionPath, string folderSolution){
       try
        {
            var appPath = Path.Combine(solutionPath, folderSolution);

            if(!Directory.Exists(appPath))
                Directory.CreateDirectory(appPath);
         
            Console.WriteLine("Qual o nome da solução?");
            var nameSolution = Console.ReadLine();

            Directory.SetCurrentDirectory(appPath);

            var files = Directory.GetFiles(appPath);
            
            if(!files.Contains($"{Path.Combine(appPath, nameSolution)}.csproj"))
            {
                Process.Start("dotnet", $"new sln -n {nameSolution} -o {appPath}").WaitForExit();
                Process.Start("dotnet", $"new mvc -n {nameSolution} -o {appPath}").WaitForExit();   
            } else{
                Console.WriteLine($"Aplicação com o mesmo nome e na mesma pasta já existe: {appPath}");
            }
            
            GenerateClassLibsCQRS(appPath, nameSolution);
            
        }catch(Exception ex)
        {
            Console.WriteLine("Ocorreu um erro ao criar a nova pasta: " + ex.Message);     
        }
    }

    public (string SolutionPath, string FolderPath) CreateDirectory(){
               
        string solutionPath = string.Empty;
        string folderSolution = string.Empty;

        do {
            Console.WriteLine("Coloque o caminho do qual você quer que a solução seja criada: ");
            solutionPath = Console.ReadLine().ToString();

            Console.WriteLine("Qual o nome da pasta em que o projeto deve ser criado");
            folderSolution = Console.ReadLine().ToString();

        } while(solutionPath.IsNullOrWhitespace() || folderSolution.IsNullOrWhitespace());

        return (solutionPath, folderSolution);
    }

    private void GenerateClassLibsCQRS(string appPath, string nameSolution){
        try{

            var folders = new List<string>(){
                "Application",
                "Domain",
                "Infrastructure"
            };

            Directory.SetCurrentDirectory(appPath);

            folders.ForEach(folder => {
                if(!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            });

        }catch(Exception ex){
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}


public static class GenerateApplication
{
    public static void InitializeConsole(){
        Console.ForegroundColor = ConsoleColor.DarkBlue;
    }
}

