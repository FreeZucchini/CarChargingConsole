<script>
  import { carStatus } from './store.js';
  const API_BASE = "https://cardashboardbackend-arg8cmfgf6befkhd.westus3-01.azurewebsites.net/api";

  let startchargeTime = $state($carStatus.scheduleTime);

  async function toggle() {
    const setSchedule = $carStatus.setSchedule;
    const endpoint = setSchedule ? "schedule/cancel" : "schedule/set"; 

    const payload = {
      time: startchargeTime
    };

    try {
        const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: "POST",
        body: JSON.stringify(payload)
      });
        if (!response.ok) {
          throw new Error(`Request failed: ${response.status}`);
        }

        carStatus.update((s) => ({
        ...s,
        setSchedule: !setSchedule,
        scheduleTime: setSchedule ? s.scheduleTime : startchargeTime
      }));
        console.log(`${endpoint} succeeded`);
    } catch (err) {
        console.log(`${endpoint} failed: `, err);
    }
  }
      
</script>

<div> 
  <button type="button" class="counter" onclick={toggle}>{$carStatus.setSchedule ? "Cancel Schedule" : "Set Schedule"}</button>
  <input type="time" bind:value={startchargeTime} required/>
</div>

