import { Utility } from "./Utility";

export interface TaskCreationData {
    name: string;
    description: string;
    startTime: string;
    endTime?: string;
    status: string;
}

export class Task implements TaskCreationData {

    static async create(task: TaskCreationData): Promise<Task> {
        const response = await fetch("api/Task", {
            method: "POST",
            body: JSON.stringify(task),
            headers: {
                "Content-Type": "application/json"
            }
        });

        Utility.checkResponse(response);

        return await response.json();
    }

    static async edit(taskId: number, task: { status: string }) {
        const response = await fetch("api/Task/" + taskId, {
            method: "PUT",
            body: JSON.stringify(task),
            headers: {
                "Content-Type": "application/json"
            }
        });

        Utility.checkResponse(response);

        return true;
    }

    static async get() {
        const response = await fetch("/api/Task");

        const result = await response.json();

        return result as Task[];
    }

    static async delete(id: number) {
        const response = await fetch("api/Task/" + id, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json"
            }
        });

        const result = await response.json();

        return result as Task[];
    }

    id: number;
    name: string;
    description: string;
    startTime: string;
    endTime?: string;
    status: string;

    constructor(id: number, name: string, description: string, startTime: string, endTime: string | undefined, status: string) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.startTime = startTime;
        this.endTime = endTime;
        this.status = status;
    }
}