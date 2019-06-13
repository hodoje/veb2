import { Station } from './station.model';

// public Station Station { get; set; }
//         public int SequenceNumber { get; set; }

export class RoutePoint {
    sequenceNumber: number;
    station: Station;
}