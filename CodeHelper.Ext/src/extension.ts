// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vsc from 'vscode';
import { CodelensProvider } from './CodelensProvider';
import * as path from 'path';
import * as fs from 'fs';
// this method is called when your extension is activated
// your extension is activated the very first time the command is executed

interface DiagnosticOut {
    Line?: number | undefined;
    Code: string | number | undefined;
    Source: string | undefined;
    Message: string | undefined;
}

let disposables: vsc.Disposable[] = [];
const filePath = 'C:\\Users\\DCorpse\\Desktop\\Projects\\Daria\\vscode-extension-samples\\codelens-sample\\out\\out.json';
let writebuffer: Array<DiagnosticOut>;

function WriteFile(data: string) {
    const stream = fs.createWriteStream(filePath);
    
    stream.on('finish', () => {
        console.log('All the data is transmitted');
    });
    
    stream.write(data);
    stream.end();
  }

export function activate(context: vsc.ExtensionContext) {

    const codelensProvider = new CodelensProvider();
    
    context.subscriptions.push(vsc.languages.onDidChangeDiagnostics(e => {
        
        let out: DiagnosticOut;
        
        const buffer = new SharedArrayBuffer(1024);
        
        if(vsc.window.activeTextEditor != null)
        {
            const uri = vsc.window.activeTextEditor?.document.uri;
            
            const list = vsc.languages.getDiagnostics(uri).map((diag) => 
            {
                return out = 
                {
                    Line: (diag.range.start.line.valueOf() + 1),
                    Code: diag.code,
                    Message: diag.message,
                    Source: diag.source
                };
            });
            
            
            if (list.length> 0 && !Equals(writebuffer, list))    
            {
                writebuffer = list;
                WriteFile(JSON.stringify(list));
                console.log(list);
            }
        }
        
    }));
    
    function Equals(a:Array<DiagnosticOut>, b: Array<DiagnosticOut> )
    {
        if(a == null || b == null) return false;
        
        let res = true;

        a.forEach((item, index) => {
            res = res && (item.Message == b.at(index)?.Message); 
        });
        return res;
    }

	const collection = vsc.languages.createDiagnosticCollection('test');
	if (vsc.window.activeTextEditor) {
		updateDiagnostics(vsc.window.activeTextEditor.document, collection);
	}
    
	context.subscriptions.push(vsc.window.onDidChangeActiveTextEditor(editor => {
		if (editor) {
			updateDiagnostics(editor.document, collection);
		}
	}));
}

function updateDiagnostics(document: vsc.TextDocument, collection: vsc.DiagnosticCollection): void {

	if (document && path.basename(document.uri.fsPath) === 'sample-demo.rs') {
		collection.set(document.uri, [{
			code: '',
			message: 'cannot assign twice to immutable variable `x`',
			range: new vsc.Range(new vsc.Position(3, 4), new vsc.Position(3, 10)),
			severity: vsc.DiagnosticSeverity.Error,
			source: '',
			relatedInformation: [
				new vsc.DiagnosticRelatedInformation(new vsc.Location(document.uri, new vsc.Range(new vsc.Position(1, 8), new vsc.Position(1, 9))), 'first assignment to `x`')
			]
		}]);
	} else {
		collection.clear();
	}
}


// this method is called when your extension is deactivated
export function deactivate() {
    if (disposables) {
        disposables.forEach(item => item.dispose());
    }
    disposables = [];
}



/*
    vsc.languages.registerCodeLensProvider("*", codelensProvider);

    vsc.commands.registerCommand("codelens-sample.enableCodeLens", () => {
        vsc.workspace.getConfiguration("codelens-sample").update("enableCodeLens", true, true);
    });

    vsc.commands.registerCommand("codelens-sample.disableCodeLens", () => {
        vsc.workspace.getConfiguration("codelens-sample").update("enableCodeLens", false, true);
    });

    vsc.commands.registerCommand("codelens-sample.codelensAction", (args: any) => {
        vsc.window.showInformationMessage(`CodeLens action clicked with args=${args}`);
    });
*/