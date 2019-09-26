export class Utility {
    static checkResponse(response: Response) {
        if (!response.ok) {
            throw { message: "Response error", response };
        }
    }
}