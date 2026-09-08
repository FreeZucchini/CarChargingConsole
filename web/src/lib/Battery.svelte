<script>
  import {onMount} from 'svelte'; 
  import { carStatus } from './store.js';

  async function getStatusFromCar() {
    const API_BASE = "https://cardashboardbackend-arg8cmfgf6befkhd.westus3-01.azurewebsites.net/api";

    try {
      const response = await fetch(`${API_BASE}/status`, {
        method: "GET"
      });
      if (!response.ok) {
        throw new Error(`Request failed: ${response.status}`);
      }
      const data = await response.json();

      carStatus.update((s) => ({
        ...s,
        charge: data.batteryPercentage,
        charging: data.isCharging,
        setSchedule: data.scheduleSet,
        scheduleTime: data.scheduledTime ?? s.scheduleTime
      }));

      console.log(`status recieved: `, data);

    } catch (err) {
      console.log("Failed to get status:", err);
    }
  }

  onMount(() => {
    const interval = setInterval(getStatusFromCar, 5000);
    getStatusFromCar();
    return () => clearInterval(interval);
  });
</script>

<style>
  .battery-bar {
    width: 200px;
    height: 40px;
    border: 3px solid black;
    border-radius: 3px;
    overflow: hidden;
    margin-right: 20px;
  }

  .fill {
    height: 100%;
    background: black;
    transition: width 0.3s ease;
  }
</style>

<div style="display: flex; flex-direction: row">
  <div class="battery-bar">
      <div class="fill" style={`width: ${$carStatus.charge}%`}></div>
  </div>
  <h2 style="font-size: 35px">{$carStatus.charge}%</h2>
</div>


