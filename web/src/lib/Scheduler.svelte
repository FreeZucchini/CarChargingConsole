<script>
  import { carStatus } from './store.js';
  const API_BASE = "https://cardashboardbackend-arg8cmfgf6befkhd.westus3-01.azurewebsites.net/api";

  let startchargeTime = $state($carStatus.scheduleTime);
  let errorMessage = $state("");

  async function toggle() {
    const setSchedule = $carStatus.setSchedule;
    const endpoint = setSchedule ? "schedule/off" : "schedule/on"; 
    errorMessage = "";

    const payload = {
      time: startchargeTime
    };
    
    try {
        const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(payload)
      });

      console.log(payload);

      const data = await response.json().catch(() => null);
      
      if (!response.ok || !data?.success) {
        throw new Error(data?.error ?? `Request failed: ${response.status}`);
      }

      carStatus.update((s) => ({
        ...s,
        setSchedule: !setSchedule,
        scheduleTime: setSchedule ? s.scheduleTime : startchargeTime
      }));
      
      console.log(`${endpoint} succeeded`);
    } catch (err) {
        errorMessage = setSchedule
          ? "Could not cancel the schedule. Please try again."
          : "Could not set the schedule. Please try again.";
        console.log(`${endpoint} failed: `, err);
    }
  }
      
</script>

<div>
  <button type="button" class="counter" onclick={toggle}>{$carStatus.setSchedule ? "Cancel Schedule" : "Set Schedule"}</button>
  <input type="time" bind:value={startchargeTime} required/>
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

