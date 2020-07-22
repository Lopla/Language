import * as vscode from 'vscode';
import * as path from 'path';
import * as execFile from 'child_process';
import { outputWindow } from './extension';

export const loplaToolPath = path.resolve(__dirname, '../resources/loplac');
export const loplaTool = path.resolve(__filename, loplaToolPath, "loplac.exe");
export const loplaScripsPath = path.resolve(__dirname, '../resources/scripts');

export let LoplaSchema = {};
export let LoplaKeywords = ['function', 'while', 'if', 'return'];
export let LoplaMethods = {};

function pupulateFunctionArgs(){
  
    for (const key of Object.keys(LoplaSchema)) {
      for (const methods of Object.keys(LoplaSchema[key])) {
        
        var m = key+"."+methods;
        var fileRun = execFile.execFileSync(loplaTool, ["/nogui", loplaScripsPath,  "function", m]);
        
        var rows = fileRun.toString().split("\n");
        LoplaMethods[m] = [];

        if(rows){
          for(var k=1;k<rows.length;k++)
          {
            var p = rows[k].trim();
            if(p)
              LoplaMethods[m].push(p);
          }
        }
      }
    }
  }
  
export   function getAvailbleFunctions(){
  
    execFile.execFile(loplaTool, ["/nogui", loplaScripsPath, "functions"], {}, (error, stdout, stderr) => {
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

      if(stdout)
        outputWindow.appendLine(stdout);

      if(stderr)
        outputWindow.appendLine(stderr);
    });
}