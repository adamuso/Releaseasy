export class User {
    static async logIn(username: string, password: string): Promise<boolean> {
        const response = await fetch("/api/User/Login", {
            method: "POST",
            body: JSON.stringify({ username, password }),
            headers: {
                "Content-Type": "application/json"
            }
        });

        const result = await response.json();

        return result;
    }

    static async register(type: "employee" | "company", name: string, lastName: string, location: string, username: string, password: string, email: string): Promise<boolean> {
        const response = await fetch("/api/User/Register", {
            method: "POST",
            body: JSON.stringify({ type, name, lastName, location, username, password, email }),
            headers: {
                "Content-Type": "application/json"
            }
        });

        const result = await response.json();

        return result;
    }

    static async get(id: string) {
        const response = await fetch("/api/User/" + id);

        const result = await response.json();

        return result as {
            name: string,
            lastName: string,
            type: string,
            location: string,
            email: string,
            emailConfirmed: string,
            id: string,
            phoneNumber: string,
            phoneNumberConfirmed: string,
            userName: string
        }[];
    }

    static async getCurrent() {
        const response = await fetch("/api/User/Current");

        const result = await response.json();

        return result as {
            name: string,
            lastName: string,
            type: string,
            location: string,
            email: string,
            emailConfirmed: string,
            id: string,
            phoneNumber: string,
            phoneNumberConfirmed: string,
            userName: string
        }[];
    }

    static async getCreatedProjects() {
        const response = await fetch("/api/User/CreatedProjects");

        const result = await response.json();

        return result as {
            id: string,
            description: string,
            endTime: string,
            name: string,
            startTime: string
        }[];
    }
}