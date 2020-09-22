import { Task } from "./Task";
import { Utility } from "./Utility";

export interface ProjectCreationData {
    name: string,
    description: string
}

export class Project implements ProjectCreationData {

    static async create(project: ProjectCreationData): Promise<Project> {
        const response = await fetch("api/Project", {
            method: "POST",
            body: JSON.stringify(project),
            headers: {
                "Content-Type": "application/json"
            }
        });

        Utility.checkResponse(response);

        return await response.json();
    }

    static async get(id: string) {
        const response = await fetch("/api/Project/" + id);

        const result = await response.json();

        return result as {
            id: number,
            creator: string,
            description: string,
            endTime: string,
            name: string,
            startTime: string,
            tasks: Task[]
        };
    }

    static async put(id: string, args: { description?: string }) {
        const response = await fetch("api/Project/" + id, {
            method: "PUT",
            body: JSON.stringify(args),
            headers: {
                "Content-Type": "application/json"
            }
        });

        Utility.checkResponse(response);

        await response.text();
    }

    static async getLastCreatedProjects() {
        const response = await fetch("/api/Project/LastCreatedProjects");

        const result = await response.json();

        return result as {
            id: number,
            description: string,
            endTime: string,
            name: string,
            startTime: string
        }[];
    }

    static async addTask(id: Project | number, task: Task | number) {
        const response = await fetch("api/Project/AddTask", {
            method: "POST",
            body: JSON.stringify({
                projectId: typeof id === "object" ? id.id : id,
                taskId: typeof task === "object" ? task.id : task
            }),
            headers: {
                "Content-Type": "application/json"
            }
        });

        Utility.checkResponse(response);

        return true;
    }


    static async removeTask(id: Project | number, task: Task | number) {
        const response = await fetch("api/Project/RemoveTask", {
            method: "POST",
            body: JSON.stringify({
                projectId: typeof id === "object" ? id.id : id,
                taskId: typeof task === "object" ? task.id : task
            }),
            headers: {
                "Content-Type": "application/json"
            }
        });

        Utility.checkResponse(response);

        return true;
    }

    id: number;
    name: string;
    description: string;

    constructor(id: number, name: string, description: string) {
        this.id = id;
        this.name = name;
        this.description = description;
    }
}