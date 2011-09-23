<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureService" generation="1" functional="0" release="0" Id="ee4a2afd-69e1-4feb-89a8-ab56bd4016be" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="PipSep:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AzureService/AzureServiceGroup/LB:PipSep:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="PipSep:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/AzureService/AzureServiceGroup/MapPipSep:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="PipSep:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/AzureService/AzureServiceGroup/MapPipSep:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="PipSep:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/AzureService/AzureServiceGroup/MapPipSep:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="PipSep:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureService/AzureServiceGroup/MapPipSep:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="PipSepInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureService/AzureServiceGroup/MapPipSepInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:PipSep:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AzureService/AzureServiceGroup/PipSep/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapPipSep:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureService/AzureServiceGroup/PipSep/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapPipSep:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureService/AzureServiceGroup/PipSep/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapPipSep:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureService/AzureServiceGroup/PipSep/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapPipSep:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureService/AzureServiceGroup/PipSep/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapPipSepInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureService/AzureServiceGroup/PipSepInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="PipSep" generation="1" functional="0" release="0" software="C:\Users\Alex\Desktop\PipSep\AzureService\bin\Debug\AzureService.csx\roles\PipSep" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;PipSep&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;PipSep&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureService/AzureServiceGroup/PipSepInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="PipSepInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="97900298-13f9-4033-884f-281846238d69" ref="Microsoft.RedDog.Contract\ServiceContract\AzureServiceContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="2fd0f1e1-7aef-4cca-9a9e-9855cc6046b3" ref="Microsoft.RedDog.Contract\Interface\PipSep:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/AzureService/AzureServiceGroup/PipSep:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>