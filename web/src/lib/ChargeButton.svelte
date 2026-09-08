<script>
  import { carStatus } from './store.js';
  const API_BASE = "https://cardashboardbackend-arg8cmfgf6befkhd.westus3-01.azurewebsites.net/api";

  async function toggle() {
    const isCharging = $carStatus.charging
    const endpoint = isCharging ? "charge/stop" : "charge/start"; 
    try {
        const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: "POST"
      });
        if (!response.ok) {
          throw new Error(`Request failed: ${response.status}`);
        }

        carStatus.update((s) => ({ ...s, charging: !isCharging }));
        console.log(`${endpoint} succeeded`);
    } catch (err) {
        console.log(`${endpoint} failed: `, err);
    }
  }
      
</script>

<button type="button" class="counter" onclick={toggle}>{$carStatus.charging ? "Charging" : "Not Charging"}</button>
