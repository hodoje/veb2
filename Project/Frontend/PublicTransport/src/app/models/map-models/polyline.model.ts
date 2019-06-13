import { GeoLocation } from "./geolocation";

export class Polyline {
    public lineNumber: number;
    public path: GeoLocation[];
    public color: string;
    public icon: any;

    constructor(path: GeoLocation[], color: string, icon: any, lineNumber: number){
        this.color = color;
        this.path = path;
        this.icon = icon;
        this.lineNumber = lineNumber;
    }

    addLocation(location: GeoLocation){
        this.path.push(location);
    }
}