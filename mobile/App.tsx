import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { StatusBar } from 'expo-status-bar';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { HomeScreen } from '@/screens/HomeScreen';
import { LinieScreen } from '@/screens/LinieScreen';
import { PolaczeniaScreen } from '@/screens/PolaczeniaScreen';
import { RozkladLiniiScreen } from '@/screens/RozkladLiniiScreen';
import { StopPickerScreen } from '@/screens/StopPickerScreen';
import { RootStackParamList } from '@/navigation/types';

const Stack = createNativeStackNavigator<RootStackParamList>();

export default function App() {
  return (
    <SafeAreaProvider>
      <NavigationContainer>
        <Stack.Navigator
          screenOptions={{
            headerStyle: { backgroundColor: '#1d4ed8' },
            headerTintColor: '#fff',
            headerTitleStyle: { fontWeight: '700' },
          }}
        >
          <Stack.Screen
            name="Home"
            component={HomeScreen}
            options={{ title: 'GdzieToSieJechało' }}
          />
          <Stack.Screen
            name="Polaczenia"
            component={PolaczeniaScreen}
            options={{ title: 'Wyszukaj połączenie' }}
          />
          <Stack.Screen
            name="StopPicker"
            component={StopPickerScreen}
            options={({ route }) => ({
              title: route.params?.title ?? 'Wybierz przystanek',
              presentation: 'modal',
            })}
          />
          <Stack.Screen
            name="Linie"
            component={LinieScreen}
            options={{ title: 'Linie' }}
          />
          <Stack.Screen
            name="RozkladLinii"
            component={RozkladLiniiScreen}
            options={{ title: 'Rozkład' }}
          />
        </Stack.Navigator>
        <StatusBar style="light" />
      </NavigationContainer>
    </SafeAreaProvider>
  );
}
