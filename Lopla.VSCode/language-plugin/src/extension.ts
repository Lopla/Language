import * as vscode from 'vscode';
import { getAvailbleFunctions, LoplaSchema, LoplaKeywords, loplaToolExists, loplaToolFile } from './intelisense';
import { LoplaTaskProvider } from './task';
import { LibsCompletetionProvider } from './libsCompletetionProvider';
import { loplaTool } from './intelisense';
import { run } from './intelisense';
import { OutputChannel } from 'vscode';
import { FunctionCompletetionProvider } from './functionCompletetionProvider';

let loplaStatusBarItem: vscode.StatusBarItem;
let loplaDocumentScheme = {scheme: 'file',language:'lopla'};
let pluginEnabled = false;

export let outputWindow: OutputChannel;

export function activate(context: vscode.ExtensionContext) {
  // check if lplc consoel exists and if the file exist
  if(!loplaToolExists()){
    vscode.window.showErrorMessage("Lopla console not found.");
    
  }else
  {
    pluginEnabled = true;
  }

  getAvailbleFunctions();

  context.subscriptions.push(
    vscode.commands.registerCommand('extension.lopla.run', () => {
          run();
      })
  );

  
  context.subscriptions.push(
    vscode.commands.registerCommand('extension.lopla.diag', () => {
        if(!pluginEnabled){
          vscode.window.showErrorMessage("Lopla console not found. "+loplaToolFile());
        }else{
          vscode.window.showInformationMessage("Lopla console is available.");
        }
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

  context.subscriptions.push(
    vscode.languages.registerSignatureHelpProvider(loplaDocumentScheme, 
      new FunctionCompletetionProvider(), "(")    
  );


  let workspaceRoot = vscode.workspace.rootPath;
  const taskProvider = vscode.tasks.registerTaskProvider("lopla", new LoplaTaskProvider(workspaceRoot));
  
  outputWindow = vscode.window.createOutputChannel("Lopla");

  loplaStatusBarItem = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 1);
  loplaStatusBarItem.command = 'extension.lopla.diag';
  loplaStatusBarItem.text = `Lopla`;
  loplaStatusBarItem.color = vscode.ThemeColor.name;
  loplaStatusBarItem.show();
	context.subscriptions.push(loplaStatusBarItem);
}

export function deactivate() {

}
