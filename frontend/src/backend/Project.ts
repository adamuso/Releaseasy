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
            id: string,
            creator: string,
            description: string,
            endTime: string,
            name: string,
            startTime: string
        };
    }

    static async getLastCreatedProjects() {
        const response = await fetch("/api/Project/LastCreatedProjects");

        const result = await response.json();

        return result as {
            id: string,
            description: string,
            endTime: string,
            name: string,
            startTime: string
        }[];
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