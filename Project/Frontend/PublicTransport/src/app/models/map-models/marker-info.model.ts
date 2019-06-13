import { GeoLocation } from './geolocation';

export class MarkerInfo {
    iconUrl: string;
    title: string;
    label: string;
    location: GeoLocation;

    constructor(location: GeoLocation, title: string, icon: string, label: string) {
        this.iconUrl = icon;
        this.location = location;
        this.title = title;
        this.label = label;
    }
}