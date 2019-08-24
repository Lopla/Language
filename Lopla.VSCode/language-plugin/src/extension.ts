import * as vscode from 'vscode';
import { getAvailbleFunctions, LoplaSchema, LoplaKeywords } from './intelisense';
import { LoplaTaskProvider } from './task';

let loplaStatusBarItem: vscode.StatusBarItem;

export function activate(context: vscode.ExtensionContext) {
  getAvailbleFunctions();

  context.subscriptions.push(
    vscode.commands.registerCommand('extension.lopla.run', () => {
      run()
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

  let workspaceRoot = vscode.workspace.rootPath;
  const taskProvider = vscode.tasks.registerTaskProvider("lopla", new LoplaTaskProvider(workspaceRoot));
  
  
  // const myCommandId = 'extension.lopla.run';
  // loplaStatusBarItem = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 1);
  // loplaStatusBarItem.command = myCommandId;
  // loplaStatusBarItem.text = `Lopla`;
  // loplaStatusBarItem.color = vscode.ThemeColor.name;
  // loplaStatusBarItem.show();
	// context.subscriptions.push(loplaStatusBarItem);
}

export function deactivate() {

}
