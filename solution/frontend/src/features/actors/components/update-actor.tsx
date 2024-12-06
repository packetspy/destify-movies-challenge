import { Pen, Plus, X } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Form, FormDrawer, Input, Select } from '@/components/ui/form'
import { useNotifications } from '@/components/ui/notifications'

import { useActor } from '../api/get-actor'
import { updateActorInputSchema, useUpdateActor } from '../api/update-actor'
import { useState } from 'react'
import { Movie } from '@/types/api'
import { useAllMovies } from '@/features/movies/api/get-movies'

type UpdateActorProps = {
  uniqueId: string
}

export const UpdateActor = ({ uniqueId }: UpdateActorProps) => {
  const { addNotification } = useNotifications()
  const actorsQuery = useActor({ uniqueId })
  const moviesAllQuery = useAllMovies({})

  const mutation = useUpdateActor({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Actor Updated',
          message: 'The actor was successfull'
        })

        actorsQuery.refetch()
      }
    }
  })

  const actor = actorsQuery.data?.data
  const [reactiveMovies, setReactiveMovies] = useState(actor?.movies || [])
  const [selectedMovie, setSelectedMovie] = useState<string | null>(null)

  const addMovie = (uniqueId: string) => {
    if (uniqueId) {
      const movie = moviesAllQuery.data?.find((movie: Movie) => movie.uniqueId === uniqueId)
      const movieExists = reactiveMovies.find((movie) => movie.uniqueId === uniqueId)
      if (movie && !movieExists) setReactiveMovies((prevMovies) => [...prevMovies, movie])
    }
    setSelectedMovie(null)
  }

  const removeMovie = (uniqueId: string) => {
    setReactiveMovies((prevMovies) => prevMovies.filter((movie) => movie.uniqueId !== uniqueId))
  }

  return (
    <FormDrawer
      isDone={mutation.isSuccess}
      triggerButton={
        <Button size="sm" icon={<Pen className="size-4" />}>
          Update Actor
        </Button>
      }
      title="Update Actor"
      submitButton={
        <>
          <Button form="update-actor" type="submit" size="sm" isLoading={mutation.isPending}>
            Submit
          </Button>
        </>
      }
    >
      <Form
        id="update-actor"
        onSubmit={(values) => {
          values.movies = reactiveMovies
          mutation.mutate({
            data: values,
            uniqueId
          })
        }}
        options={{
          defaultValues: {
            uniqueId: actor?.uniqueId ?? '',
            movies: actor?.movies ?? [],
            name: actor?.name ?? ''
          }
        }}
        schema={updateActorInputSchema}
      >
        {({ register, formState }) => {
          return (
            <>
              <Input
                label="Name"
                error={formState.errors['name']}
                registration={register('name')}
              />

              <div className="grid grid-cols-2 gap-4 content-end">
                <Select
                  label="Movies"
                  className="border border-gray-300 rounded-md"
                  defaultValue={null}
                  options={(moviesAllQuery?.data ?? []).map((movie: Movie) => ({
                    value: movie.uniqueId,
                    label: movie.title
                  }))}
                  registration={register('movies', {
                    onChange: (e) => setSelectedMovie(e.target.value)
                  })}
                ></Select>
                <div className="content-end">
                  <Button
                    type="button"
                    icon={<Plus className="size-4" />}
                    onClick={() => selectedMovie && addMovie(selectedMovie)}
                  >
                    Add
                  </Button>
                </div>
              </div>

              <ul>
                {reactiveMovies.map((movie, index) => (
                  <li key={index} className="ml-5 text-gray-700 list-none flex ">
                    {movie.title}
                    <Button
                      type="button"
                      onClick={() => removeMovie(movie.uniqueId)}
                      className="content-center size-1 m-1"
                    >
                      <X className="size-4" />
                    </Button>
                  </li>
                ))}
              </ul>
            </>
          )
        }}
      </Form>
    </FormDrawer>
  )
}
