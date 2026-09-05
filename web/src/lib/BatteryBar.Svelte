<script>
    let { charge = 0 } = $props()
</script>

<style>
  .battery-bar {
    width: 100px;
    height: 20px;
    border: 2px solid black;
    border-radius: 3px;
    overflow: hidden;
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

