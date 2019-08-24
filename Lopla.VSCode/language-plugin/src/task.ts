import * as vscode from 'vscode';
import { TaskScope, TaskDefinition } from 'vscode';
import { loplaTool } from './intelisense';

import * as path from 'path';

export class LoplaTaskProvider implements vscode.TaskProvider{
    private tasks: vscode.Task[] | undefined;
    constructor(private _workspaceRoot: string) {
		
    }
    
    provideTasks(token?: vscode.CancellationToken): vscode.ProviderResult<vscode.Task[]> {
        // if(this.tasks == undefined)
        // {
        //     this.tasks = []; 
        //     let def = {}
        //     this.tasks.push(this.getTask({}));
        // }

        return undefined;
    }      

    getTask(definition: TaskDefinition): vscode.Task {
        return new vscode.Task(definition, TaskScope.Workspace,  "run", "lopla", 
            new vscode.ShellExecution(loplaTool,{
                shellArgs:[ this._workspaceRoot ]
            }));
    }
    
    resolveTask(_task: vscode.Task, token?: vscode.CancellationToken): vscode.ProviderResult<vscode.Task> {        
        return this.getTask(_task.definition);
    }
}

interface LoplaTaskDefinition extends vscode.TaskDefinition {
}