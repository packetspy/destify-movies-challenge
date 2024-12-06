import { ContentLayout } from '@/components/layouts'

export const DashboardRoute = () => {
  return (
    <ContentLayout title="Dashboard">
      <h1 className="text-xl">Welcome to the dashboard page</h1>
      <p className="font-medium">Please, use side-menu to navigate.</p>
    </ContentLayout>
  )
}
