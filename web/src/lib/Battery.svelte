<script>
  import {onMount} from 'svelte'; 
  let { charge = 0 } = $props()

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
      charge = data.batteryPercentage;
      console.log(`status recieved: `, data);
    } catch (err) {
      console.log("Failed to get status:", err);
    }
  }

  onMount(() => {
    const interval = setInterval(getStatusFromCar, 5000);

    return () => {
      clearInterval(interval)
    };
  });
</script>

<style>
  .battery-bar {
    width: 200px;
    height: 40px;
    border: 3px solid black;
    border-radius: 3px;
    overflow: hidden;
    margin: 0 auto;
  }

  .fill {
    height: 100%;
    background: black;
    transition: width 0.3s ease;
  }
</style>

<div class="battery-bar">
    <div class="fill" style={`width: ${charge}%`}></div>
</div>

