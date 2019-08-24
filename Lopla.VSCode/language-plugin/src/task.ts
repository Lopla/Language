import * as vscode from 'vscode';



export class LoplaTaskProvider implements vscode.TaskProvider{
    private tasks: vscode.Task[] | undefined;

    provideTasks(token?: vscode.CancellationToken): vscode.ProviderResult<vscode.Task[]> {
        if(this.tasks == undefined)
        {
            this.tasks = []; 
            this.tasks.push(this.getTask());
        }

        return this.tasks;
    }      

    getTask(): vscode.Task {
        const definition: LoplaTaskDefinition = <any>null;
        return new vscode.Task(definition, definition.task, 'lopla', new vscode.ShellExecution(`lopla ${definition.task}`));
    }
    
    resolveTask(_task: vscode.Task, token?: vscode.CancellationToken): vscode.ProviderResult<vscode.Task> {
        const task = _task.definition.task;
		// A Rake task consists of a task and an optional file as specified in RakeTaskDefinition
		// Make sure that this looks like a Rake task by checking that there is a task.
		if (task) {
			// resolveTask requires that the same definition object be used.
			const definition: LoplaTaskDefinition = <any>_task.definition;
			return new vscode.Task(definition, definition.task, 'lopla', new vscode.ShellExecution(`lopla ${definition.task}`));
		}
		return undefined;
    }
}


interface LoplaTaskDefinition extends vscode.TaskDefinition {
}