import {writable} from 'svelte/store';

function loadInitial() {
    return {
        charge: Number(localStorage.getItem("charge")) || 0,
        charging: localStorage.getItem("charging") === "true",
        setSchedule: localStorage.getItem("setSchedule") === "true",
        scheduleTime: localStorage.getItem("scheduleTime") || ""
    };
}

export const carStatus = writable((loadInitial()));

carStatus.subscribe((value) => {
    localStorage.setItem("charge", value.charge.toString());
    localStorage.setItem("charging", value.charging.toString());
    localStorage.setItem("setSchedule", value.setSchedule.toString());
    localStorage.setItem("scheduleTime", value.scheduleTime);
});
