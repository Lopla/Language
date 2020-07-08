import * as vscode from 'vscode';
import * as path from 'path';
import * as execFile from 'child_process';
import { outputWindow } from './extension';

export const loplaToolPath = path.resolve(__dirname, '../resources/loplac');
export const loplaTool = path.resolve(__filename, loplaToolPath, "loplac.exe");

export let LoplaSchema = {};
export let LoplaKeywords = ['function', 'while', 'if', 'return'];

function pupulateFunctionArgs(){
    for (const key of Object.keys(LoplaSchema)) {
      for (const methods of Object.keys(LoplaSchema[key])) {
        //console.log(key+"."+methods)
      }
    }
  }
  
export   function getAvailbleFunctions(){
    var loplaScripsPath = path.resolve(__dirname, '../resources/scripts');
  
    execFile.execFile(loplaTool, [loplaScripsPath, "functions"], {}, (error, stdout, stderr) => {
      var r = new RegExp("([a-zA-Z]+)[.]([a-zA-Z]+)")
      
      if(stdout){
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


export function run(){
    var currentlyOpenTabfilePath = vscode.window.activeTextEditor.document.fileName;
    var currentlyOpenTabfileName = path.dirname(currentlyOpenTabfilePath);

    execFile.execFile(loplaTool, [currentlyOpenTabfileName], {}, (error, stdout, stderr) => {
      if(error || stderr){
        var e = error || stderr;
        vscode.window.showErrorMessage(e.toString());
      }

      outputWindow.appendLine(stdout);
      outputWindow.appendLine(stderr);
    });
}