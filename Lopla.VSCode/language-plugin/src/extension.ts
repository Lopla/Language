import * as vscode from 'vscode';
import * as path from 'path';
import * as execFile from 'child_process';

let runTaskProvider: vscode.Disposable | undefined;

let LoplaSchema = {};
let LoplaKeywords = ['function', 'while', 'if', 'return'];

function pupulateFunctionArgs(){
  for (const key of Object.keys(LoplaSchema)) {
    for (const methods of Object.keys(LoplaSchema[key])) {
      console.log(key+"."+methods)
    }
  }
}

function getAvailbleFunctions(){
  var loplaToolPath = path.resolve(__dirname, '../resources/loplad');
  var loplaTool = path.resolve(__filename, loplaToolPath, "loplad.exe");
  var loplaScripsPath = path.resolve(__dirname, '../resources/scripts');

  execFile.execFile(loplaTool, [loplaScripsPath, "functions"], {}, (error, stdout, stderr) => {
    var r = new RegExp("([a-zA-Z]+)[.]([a-zA-Z]+)")
    
    if(stdout){
      console.log(stdout);
      
      var lines = stdout.split("\n");
      lines.forEach(function(line){
        let t = line.trim();
        var e = r.exec(t);
        
        if(e && e.length > 2)
        {
          var schema = e[1];
          var method = e[2];
          if(!LoplaSchema[schema])
             LoplaSchema[schema] = {};
          LoplaSchema[schema][method] = {};
        }
      })
    }

    pupulateFunctionArgs();

  });
}

export function activate(context: vscode.ExtensionContext) {
  getAvailbleFunctions();

  context.subscriptions.push(
    vscode.commands.registerCommand('extension.lopla', () => {
      vscode.window.showInformationMessage('Lopla extension loaded');
    })
  );

  var loplaDocumentScheme = {scheme: 'file',language:'lopla'};

  context.subscriptions.push(
    vscode.languages.registerHoverProvider(loplaDocumentScheme, {
      provideHover(document, position, token) {
        return {
          contents: ['Lopla file']
        };
      }
    })
  );

  context.subscriptions.push(
    vscode.languages.registerCompletionItemProvider(loplaDocumentScheme, {
      provideCompletionItems(document: vscode.TextDocument, position: vscode.Position) {
        let suggestions = [];
        
        for (const key of Object.keys(LoplaSchema)) {
          suggestions.push(new vscode.CompletionItem(key, vscode.CompletionItemKind.Module))
        }
        for (const val of LoplaKeywords) {
          suggestions.push(new vscode.CompletionItem(val, vscode.CompletionItemKind.Keyword))
        }

        return suggestions;
      }
    })
  );

  context.subscriptions.push(
    vscode.languages.registerCompletionItemProvider(loplaDocumentScheme, {
      provideCompletionItems(document: vscode.TextDocument, position: vscode.Position) {
        
        let p = position.translate(0, -position.character);
        let line = document.getText(new vscode.Range(p, position));

        for (const key of Object.keys(LoplaSchema)) {
          let rs = '\\b' + key +'\\.';
          let r= new RegExp(rs);
          
          if(line && r.test(line)){
            var suggestions = [];
            
            var methods = Object.keys(LoplaSchema[key]);
            methods.forEach(element => {
              suggestions.push(new vscode.CompletionItem(element, vscode.CompletionItemKind.Function));
            })

            return suggestions;
          }
        }
        
        return null;
      }
    }, ".")
  );

  runTaskProvider = vscode.tasks.registerTaskProvider(runTaskProvider.RakeType, new RakeTaskProvider(workspaceRoot));
}

export function deactivate() {

}
