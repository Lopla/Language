import * as vscode from 'vscode';
import { TaskScope, TaskDefinition } from 'vscode';
import { loplaTool } from './intelisense';

export class LoplaTaskProvider implements vscode.TaskProvider{
    private tasks: vscode.Task[] | undefined;
    constructor(private _workspaceRoot: string) {
		
    }
    
    provideTasks(token?: vscode.CancellationToken): vscode.ProviderResult<vscode.Task[]> {
        let tasks: vscode.Task[] = [];

        let d: TaskDefinition={
            type:"lopla"
        };
        tasks.push(this.getTask(d));

        return tasks;
    }      

    
    getTask(definition: TaskDefinition): vscode.Task {
        const def: LoplaTaskDefinition = <any>definition;

        let startingFolder:string = this._workspaceRoot;
        if(def && def.folder){
            startingFolder = def.folder.toString();
        }
        
        return new vscode.Task(definition, TaskScope.Workspace,  "run", "lopla", 
                new vscode.ShellExecution(
                    loplaTool, 
                    [startingFolder]));
    }
    
    resolveTask(_task: vscode.Task, token?: vscode.CancellationToken): vscode.ProviderResult<vscode.Task> {        
        return this.getTask(_task.definition);
    }
}

interface LoplaTaskDefinition extends vscode.TaskDefinition {
    folder?: String;
}