<script>
  import { carStatus } from './store.js';
  const API_BASE = "https://cardashboardbackend-arg8cmfgf6befkhd.westus3-01.azurewebsites.net/api";

  let errorMessage = $state("");

  async function toggle() {
    const isCharging = $carStatus.charging
    const endpoint = isCharging ? "charge/stop" : "charge/start"; 
    errorMessage = "";

    try {
        const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: "POST"
      });

      const data = await response.json().catch(() => null);

      if (!response.ok) {
        throw new Error(data?.error ?? `Request failed: ${response.status}`);
      }

      carStatus.update((s) => ({ ...s, charging: !isCharging }));
      console.log(`${endpoint} succeeded`);
    } catch (err) {
        errorMessage = `Could not ${isCharging ? "stop" : "start"} charging. Please try again.`;
        console.log(`${endpoint} failed: `, err);
    }
  }
      
</script>

<div>
  <button type="button" class="counter" onclick={toggle}>{$carStatus.charging ? "Charging" : "Not Charging"}</button>
  {#if errorMessage}
    <p class="error">{errorMessage}</p>
  {/if}
</div>

<style>
  .error {
    color: #c0392b;
    font-size: 0.9rem;
    margin-top: 4px;
  }
</style>
