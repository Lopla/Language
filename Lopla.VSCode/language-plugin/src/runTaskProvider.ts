import * as path from 'path';
import * as fs from 'fs';
import * as cp from 'child_process';
import * as vscode from 'vscode';

export class RunTaskProvider implements vscode.TaskProvider {
	static LoplaType: string = 'lopla';
	private rakePromise: Thenable<vscode.Task[]> | undefined = undefined;

	constructor(workspaceRoot: string) {
		let pattern = path.join(workspaceRoot, 'Rakefile');
		let fileWatcher = vscode.workspace.createFileSystemWatcher(pattern);
		fileWatcher.onDidChange(() => this.rakePromise = undefined);
		fileWatcher.onDidCreate(() => this.rakePromise = undefined);
		fileWatcher.onDidDelete(() => this.rakePromise = undefined);
	}

	public provideTasks(): Thenable<vscode.Task[]> | undefined {
		// if (!this.rakePromise) {
		// 	this.rakePromise = getRakeTasks();
		// }
        return this.rakePromise;
        
	}

	public resolveTask(_task: vscode.Task): vscode.Task | undefined {
		const task = _task.definition.task;
		// A Rake task consists of a task and an optional file as specified in RakeTaskDefinition
		// Make sure that this looks like a Rake task by checking that there is a task.
		if (task) {
			// resolveTask requires that the same definition object be used.
			const definition: RakeTaskDefinition = <any>_task.definition;
			return new vscode.Task(definition, definition.task, 'rake', new vscode.ShellExecution(`rake ${definition.task}`));
		}
		return undefined;
	}
}

function exists(file: string): Promise<boolean> {
	return new Promise<boolean>((resolve, _reject) => {
		fs.exists(file, (value) => {
			resolve(value);
		});
	});
}