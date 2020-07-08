import * as vscode from 'vscode';
import * as execFile from 'child_process';
import { getAvailbleFunctions, LoplaSchema, LoplaKeywords } from './intelisense';
import { LoplaTaskProvider } from './task';
import { LibsCompletetionProvider } from './libsCompletetionProvider';
import { loplaTool } from './intelisense';
import { run } from './intelisense';

let loplaStatusBarItem: vscode.StatusBarItem;
let loplaDocumentScheme = {scheme: 'file',language:'lopla'};

export function activate(context: vscode.ExtensionContext) {
  getAvailbleFunctions();

  context.subscriptions.push(
    vscode.commands.registerCommand('extension.lopla.run', () => {
          // execFile.execFile(loplaTool, [""], {}, (error, stdout, stderr) => 
          // {
          //   console.log(stdout);
          // })
          run();
      })
  );

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
    vscode.languages.registerCompletionItemProvider(loplaDocumentScheme, 
      new LibsCompletetionProvider(), ".")    
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
