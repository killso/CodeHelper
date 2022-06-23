using CodeHelper.Service.Core;
using CodeHelper.Service.Modules.Default.Search;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Speech.Synthesis;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

namespace CodeHelper.Service;

public class Worker : BackgroundService
{

    public Worker(ILogger<Worker> logger)
    {
        LastQuery = new Query();
        Processes = new List<Process>();
        _logger = logger;
    }

    
    private readonly ILogger<Worker> _logger;

    readonly ProcessStartInfo _psiInput =
        new(fileName: @"C:\Users\DCorpse\source\repos\WinFormsAppTest\WinFormsAppTest\bin\Debug\net6.0-windows\WinFormsAppTest.exe")
        {
            RedirectStandardOutput = true
        };
    
    readonly ProcessStartInfo _psiRec =
       new(fileName: "python.exe", arguments: "c:\\users\\Dcorpse\\Desktop\\projects\\daria\\py\\script.py")
       {
           RedirectStandardOutput = true
       };

    List<Process> Processes { get; set; }

    Query LastQuery { get; set; }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Process pRec;
        var synthesizer = new SpeechSynthesizer();
        synthesizer.SetOutputToDefaultAudioDevice();
        var pCurrent = Process.GetCurrentProcess();
        var pInput = StartProcess(_psiInput, prioriry: ProcessPriorityClass.Normal);
        var deviceInput = "";
        var recOutput = "";
        var key = "";
        var search = new Search();
        var path = @"C:\Users\DCorpse\Desktop\Projects\Daria\vscode-extension-samples\codelens-sample\out\out.json";


        //search.OpenBrouser();


        WaitProcess(pInput, 100);
        while (!stoppingToken.IsCancellationRequested && !pInput.HasExited)
        {

            if (await pInput.StandardOutput.ReadLineAsync() == "Init")
            {
                LogInf(deviceInput);
                pRec = Process.Start(_psiRec);
            }
            else continue;

            WaitProcess(pRec, 100);

            recOutput = await pRec.StandardOutput.ReadToEndAsync();

            LogInf(recOutput);

            key = GetKeys(recOutput);

            LogInf(key);

            if (key != "recFail")
                TryGetInf(synthesizer, path, key);
            else recFail(synthesizer);

        }
        pInput.Close();

        pCurrent.Kill();
    }

    void recFail(SpeechSynthesizer synth)
    {
        synth.SpeakAsync("Повторите пожалуйста ваш вопрос");
    }

    string GetKeys(string? input)
    {
        string pattern = "(\\d+)";
        if (input.Contains("рауз"))
        {
            return input;
        }
        else return
                input is null ? "recFail" :
                Regex.Match(input, pattern, RegexOptions.Compiled).Value;
    }
    string ResourceRequest(string errorCode)
    {
        return $"https://stackoverflow.com/search?q={errorCode}";
    }
    async void TryGetInf(SpeechSynthesizer synth, string path, string key)
    {

        if (int.TryParse(key, out int k) && k == 0)
        {
            recFail(synth);
            return;
        }
        else if (key.Contains("рауз") && LastQuery.IsInitialized)
        {
            var qw = LastQuery.Diagnostic.ElementAt(0);
            Processes.Add(new Search().OpenBrouser(ResourceRequest(qw.Source + $"({qw.Code})")));
            return;
        }

        using var sr = new StreamReader(path: path, options: new FileStreamOptions()
        {
            Access = FileAccess.ReadWrite,
            Mode = FileMode.OpenOrCreate
        });
        var buffer = await sr.ReadToEndAsync();
        var res = JsonConvert.DeserializeObject<List<DiagnosticOut>>(buffer);
        var phrases = res.Where((item) => item.Line == k);

        LastQuery.Diagnostic.AddRange(phrases);

        synth.SpeakAsync(phrases.ElementAt(0).Message);

        Console.WriteLine($"Key: {buffer}");
    }

    void LogInf(string? input)
    {
        if (input == null)
            _logger.LogWarning(message: "Empty INPUT");
        else
            _logger.Log(LogLevel.Information, message: input);
    }

    [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, ref uint pcchOut);

    [Flags]
    enum AssocF : uint
    {
        None = 0,
        Init_NoRemapCLSID = 0x1,
        Init_ByExeName = 0x2,
        Open_ByExeName = 0x2,
        Init_DefaultToStar = 0x4,
        Init_DefaultToFolder = 0x8,
        NoUserSettings = 0x10,
        NoTruncate = 0x20,
        Verify = 0x40,
        RemapRunDll = 0x80,
        NoFixUps = 0x100,
        IgnoreBaseClass = 0x200,
        Init_IgnoreUnknown = 0x400,
        Init_FixedProgId = 0x800,
        IsProtocol = 0x1000,
        InitForFile = 0x2000,
    }

    enum AssocStr
    {
        Command = 1,
        Executable,
        FriendlyDocName,
        FriendlyAppName,
        NoOpen,
        ShellNewValue,
        DDECommand,
        DDEIfExec,
        DDEApplication,
        DDETopic,
        InfoTip,
        QuickTip,
        TileInfo,
        ContentType,
        DefaultIcon,
        ShellExtension,
        DropTarget,
        DelegateExecute,
        SupportedUriProtocols,
        Max,
    }

    void WaitProcess(Process process, int msec)
    {
        if (Process.GetProcessById(process.Id) is null)
            Thread.Sleep(msec);
        else return;
        WaitProcess(process, msec);
    }


    public Process StartProcess(ProcessStartInfo psi, ProcessPriorityClass prioriry)
    {
        string ext = Path.GetExtension(psi.FileName); //get extension

        var sb = new StringBuilder(500); //buffer for exe file path
        uint size = 500; //buffer size

        /*Get associated app*/
        uint res = AssocQueryString(AssocF.None, AssocStr.Executable, ext, null, sb, ref size);

        if (res != 0)
        {
            Debug.WriteLine("AssocQueryString returned error: " + res.ToString("X"));
        }
        else
        {
            ///psi = new ProcessStartInfo(psi.FileName, ) { RedirectStandardOutput = psi.RedirectStandardOutput };
        }
        var prc = Process.Start(psi);

        prc.PriorityClass = prioriry;

        return prc; //actually start process
    }
}
