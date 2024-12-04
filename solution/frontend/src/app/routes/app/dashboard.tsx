import { ContentLayout } from '@/components/layouts';

export const DashboardRoute = () => {
  return (
    <ContentLayout title="Dashboard">
      <h1 className="text-xl">
        Welcome <b></b>
      </h1>
      <h4 className="my-3">
        Your role is : <b></b>
      </h4>
      <p className="font-medium">In this application you can:</p>
    </ContentLayout>
  );
};